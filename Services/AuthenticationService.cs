﻿using ExtracurricularActivitiesManagement.Data;
using ExtracurricularActivitiesManagement.Models;
using ExtracurricularActivitiesManagement.Services.ServicesInterfaces;
using ExtracurricularActivitiesManagement.ViewModels.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ExtracurricularActivitiesManagement.Services
{
    public class AuthenticationService : IAuthenticationService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly ApplicationDbContext _context;
		private readonly IConfiguration _configuration;


		public AuthenticationService(UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			ApplicationDbContext context,
			IConfiguration configuration)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_context = context;
			_configuration = configuration;
		}

		public async Task<ServiceResponse<RegisterResponse, IEnumerable<IdentityError>>> RegisterUser(RegisterRequest registerRequest)
		{
			var user = new ApplicationUser
			{
				Email = registerRequest.Email,
				UserName = registerRequest.Email,
				SecurityStamp = Guid.NewGuid().ToString()
			};

			var serviceResponse = new ServiceResponse<RegisterResponse, IEnumerable<IdentityError>>();
			var result = await _userManager.CreateAsync(user, registerRequest.Password);
			if (result.Succeeded)
			{
				serviceResponse.ResponseOk = new RegisterResponse { ConfirmationToken = user.SecurityStamp };
				return serviceResponse;
			}

			serviceResponse.ResponseError = result.Errors;
			return serviceResponse;
		}

		public async Task<bool> ConfirmUserRequest(ConfirmUserRequest confirmUserRequest)
		{
			var toConfirm = _context.Users
				.Where(u => u.Email == confirmUserRequest.Email && u.SecurityStamp == confirmUserRequest.ConfirmationToken)
				.FirstOrDefault();
			if (toConfirm != null)
			{
				toConfirm.EmailConfirmed = true;
				_context.Entry(toConfirm).State = EntityState.Modified;
				await _context.SaveChangesAsync();

				return true;
			}

			return false;
		}

		public async Task<ServiceResponse<LoginResponse, string>> LoginUser(LoginRequest loginRequest)
		{
			var serviceResponse = new ServiceResponse<LoginResponse, string>();
			var user = await _userManager.FindByEmailAsync(loginRequest.Email);
			if (user != null && await _userManager.CheckPasswordAsync(user, loginRequest.Password))
			{
				var claims = new[] {
					new Claim(JwtRegisteredClaimNames.Sub, user.UserName)
                    //new Claim(JwtRegisteredClaimNames.na)
                };
				var signinKey = new SymmetricSecurityKey(
				  Encoding.UTF8.GetBytes(_configuration["Jwt:SigningKey"]));

				int expiryInMinutes = Convert.ToInt32(_configuration["Jwt:ExpiryInMinutes"]);

				var token = new JwtSecurityToken(
				  issuer: _configuration["Jwt:Site"],
				  audience: _configuration["Jwt:Site"],
				  expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
				  signingCredentials: new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256),
				  claims: claims
				);

				serviceResponse.ResponseOk = new LoginResponse { Token = new JwtSecurityTokenHandler().WriteToken(token), Expiration = token.ValidTo };
			}

			return serviceResponse;
		}

		public async Task<ServiceResponse<AuthenticationUserResponse, string>> GetCurrentUser(string value)
		{
			var user = await _userManager.FindByNameAsync(value);

			var serviceResponse = new ServiceResponse<AuthenticationUserResponse, string>();

			if (user == null)
			{
				serviceResponse.ResponseError = "No such user.";
			}

			var viewModel = new AuthenticationUserResponse
			{
				Id = user.Id,
				Email = user.Email,
				UserName = user.UserName
			};

			serviceResponse.ResponseOk = viewModel;

			return serviceResponse;
		}
	}
}
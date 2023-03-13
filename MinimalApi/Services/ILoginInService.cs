using System;
using MinimalApiDemo.Models;

namespace MinimalApiDemo.Services
{
	public interface ILoginInService
	{
		IResult Login(Login user);
	}
}


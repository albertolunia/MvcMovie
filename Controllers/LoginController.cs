﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;
using MvcMovie.Utils;

namespace MvcMovie.Controllers;

public class LoginController : Controller
{
    private readonly MvcMovieContext _context;
    private readonly AuthService _authService;

    public LoginController(MvcMovieContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
    }

    // GET: User/Login
    public IActionResult Index()
    {
        return View();
    }
    //Post: User/Login
    [HttpPost("Login")]
    public async Task<IActionResult> Login([Bind("Email,Password")] User user)
    {
        string _token = "";
        user.Password = Utils.Utils.EncryptPassword(user.Password);
        var userDb = await _context.User.FirstOrDefaultAsync(u => u.Email == user.Email && u.Password == user.Password);
        if (userDb != null)
        {
            if (userDb.Email == "admin@admin.com")
            {
                _token = _authService.GenerateJwtToken(user.Email, "admin");
                return RedirectToAction("Index", "Admin");
            }
            _token = _authService.GenerateJwtToken(user.Email, "user");
            return RedirectToAction("Index", "Open");
        }
        return RedirectToAction("Index", "Error");
    }
}
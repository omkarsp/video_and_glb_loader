using System;
using UnityEngine;

public static class AccountManager
{
    public static bool SignUp(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) return false;
        if (UserExists(username)) return false;
        string hash = HashPassword(password);
        PlayerPrefs.SetString(username, hash);
        PlayerPrefs.Save();
        return true;
    }

    public static bool Login(string username, string password)
    {
        //login if user exists
        if (!UserExists(username)) return false;
        string stored = PlayerPrefs.GetString(username);
        return stored == HashPassword(password);
    }

    public static bool UserExists(string username)
    {
        return PlayerPrefs.HasKey(username);
    }

    private static string HashPassword(string password)
    {
        if (password == null) password = "";
        var bytes = System.Text.Encoding.UTF8.GetBytes(password);
        using (var sha = System.Security.Cryptography.SHA256.Create())
        {
            var h = sha.ComputeHash(bytes);
            return Convert.ToBase64String(h);
        }
    }
}
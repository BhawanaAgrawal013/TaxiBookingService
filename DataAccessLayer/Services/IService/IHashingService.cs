﻿namespace DataAccessLayer
{
    public interface IHashingService
    {
        string Hashing(string password);
        bool VerifyHashing(string password, string passwordHash);
    }
}

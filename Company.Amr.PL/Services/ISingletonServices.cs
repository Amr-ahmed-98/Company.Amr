﻿namespace Company.Amr.PL.Services
{
    public interface ISingletonServices
    {
        public Guid Guid { get; set; }
        string GetGuid();
    }
}

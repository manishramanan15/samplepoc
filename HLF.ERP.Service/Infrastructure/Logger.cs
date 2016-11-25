using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace HLF.ERP.Services.Infrastructure
{
    public interface ILogger
    {
        void Write(string message, params object[] args);
    }

    public class Logger : ILogger
    {
       
        public void Write(string message, params object[] args)
        {
            Debug.WriteLine(message, args);
        }
    }
}
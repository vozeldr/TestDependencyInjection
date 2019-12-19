using System;
using ClassLibrary1;

namespace TestProject1
{
    public class MockOutput: IOutput
    {
        public bool Called { get; private set; }
        
        public string With { get; private set; }
        
        public void Write(string content)
        {
            Called = true;
            With = content;
        }
    }
}
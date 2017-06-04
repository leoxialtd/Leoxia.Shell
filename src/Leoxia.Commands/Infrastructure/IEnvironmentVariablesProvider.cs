using System;
using System.Collections.Generic;

namespace Leoxia.Commands
{
    public interface IEnvironmentVariablesProvider
    {
        IEnumerable<EnvironmentVariable> GetVariables();
    }

    public class EnvironmentVariablesProvider : IEnvironmentVariablesProvider
    {
        public IEnumerable<EnvironmentVariable> GetVariables()
        {
            var variables = new List<EnvironmentVariable>();
            var dictionary = Environment.GetEnvironmentVariables();
            foreach (object key in dictionary.Keys)
            {
                variables.Add(new EnvironmentVariable{ Key = key.ToString(), Value = dictionary[key].ToString() });
            }
            return variables;
        }
    }
}
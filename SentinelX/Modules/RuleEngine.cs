using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using SentinelX.Models;

namespace SentinelX.Modules
{
    public class RuleEngine
    {
        public List<Rule> Rules { get; private set; }
        public RuleEngine()
        {
            Rules = new List<Rule>();
            LoadRules();
        }

        public void LoadRules()
        {
            Rules.Clear();
            string rulesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "rules");
            if (!Directory.Exists(rulesDir))
                Directory.CreateDirectory(rulesDir);
            foreach (var file in Directory.GetFiles(rulesDir, "*.rule"))
            {
                try
                {
                    string json = File.ReadAllText(file);
                    var rule = JsonConvert.DeserializeObject<Rule>(json);
                    if (rule != null)
                        Rules.Add(rule);
                }
                catch { }
            }
        }

        public bool CheckMessage(string message, out Rule matchedRule)
        {
            foreach (var rule in Rules)
            {
                if (!string.IsNullOrEmpty(rule.Keyword) && message.IndexOf(rule.Keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    matchedRule = rule;
                    return true;
                }
                if (!string.IsNullOrEmpty(rule.RegexPattern))
                {
                    if (Regex.IsMatch(message, rule.RegexPattern, RegexOptions.IgnoreCase))
                    {
                        matchedRule = rule;
                        return true;
                    }
                }
            }
            matchedRule = null;
            return false;
        }
    }

    public class Rule
    {
        public string Name { get; set; }
        public string Keyword { get; set; }
        public string RegexPattern { get; set; }
        public string Severity { get; set; }
        public string Description { get; set; }
    }
} 
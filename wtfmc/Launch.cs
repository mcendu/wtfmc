using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wtfmc
{
    public class Launch
    {
        public Launch(WTFConfig config)
        {
            this.config = config;
        }

        private WTFConfig config;

        public string[] args;

        /// <summary>
        /// Start the game.
        /// </summary>
        public void Start(string profID)
            => Start((from p in config.Profiles
                      where p.Key == profID
                      select p.Value).First());

        /// <summary>
        /// Start the game.
        /// </summary>
        public void Start(Profile prof)
        {
            
        }
    }
}

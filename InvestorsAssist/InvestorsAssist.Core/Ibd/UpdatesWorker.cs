﻿using InvestorsAssist.DataAccess;
using InvestorsAssist.Utility.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvestorsAssist.Core.Ibd
{
    public class UpdatesWorker : IWorker
    {
        private readonly DataContext _context;

        public UpdatesWorker(DataContext context)
        {
            _context = context;
        }
        public void DoWork()
        {
            var reader = new Reader();
            var ibdPicks = reader.DownloadIbd50List();
            Logger.Instance.InfoFormat("{0} picks downloaded from investors.com", ibdPicks.Count);
            foreach (var ibdPick in ibdPicks)
            {
                _context.SaveIbdPick(ibdPick);
            }
        }

        public string Name
        {
            get { return "IBD updates"; }
        }
    }
}

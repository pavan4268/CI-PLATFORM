﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModels
{
    public class Pager
    {
        public int TotalItems { get; private set; }

        public int CurrentPage { get; private set; }

        public int PageSize { get; private set; }

        public int TotalPages { get; private set; }

        public int StartPage { get; private set; }

        public int EndPage { get; private set; }



        public Pager()
        {

        }

        public Pager(int totalitems, int page, int pagesize = 3)
        {
            int totalPages = (int)Math.Ceiling((decimal)totalitems / (decimal)pagesize);
            int currentPage = page;

            int startPage = currentPage - 5;
            int endPage = currentPage + 4;

            if (startPage <= 0)
            {
                endPage = endPage -(startPage - 1);
                startPage = 1;
            }
            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }
            TotalItems = totalitems;
            CurrentPage = currentPage;
            PageSize = pagesize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
        }


    }
}

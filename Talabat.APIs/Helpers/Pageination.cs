using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Talabat.APIs.Helpers
{
	public class Pageination<T>
	{
		public  int PageIndex  { get; set; }
        public int PageSize { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }

		public Pageination(int pageIndex, int pageSize,int count, IReadOnlyList<T> data)
		{
			PageIndex = pageIndex;
			PageSize = pageSize;
			Data = data;
			Count = count;
		}

	

        
    }
}

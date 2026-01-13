using System;
using System.Collections.Generic;

namespace Common.Interface
{
    public interface IRecordable<T>
    {
        List<T> Records { get; set; }
    }
}

using System;
using System.Collections.Generic;
using Common.Abstract;
using UnityEngine;

namespace Common.Interface
{
    public interface IGetSelfPartable
    {
        T GetPart<T>() where T : Component;
    }
}

﻿using System;
using LinFu.IoC.Configuration;

namespace SampleLibrary.IOC
{
    [Implements(typeof(ISampleGenericService<int>), ServiceName = "SpecificGenericService")]
    [Implements(typeof(ISampleGenericService<double>), ServiceName = "SpecificGenericService")]
    public class SampleGenericClassWithSpecificGenericTypeImplementation<T> : ISampleGenericService<T>
    {
        public bool Called => throw new NotImplementedException();
    }
}
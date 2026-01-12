using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neyro.NeyroNet
{
    enum MemoryMode
    {
        GET,
        SET,
        INIT
    }

    enum NeuronType
    {
        Hidden,
        Output
    }

    enum NetworkMode
    {
        Train,
        Test,
        Demo
    }
}

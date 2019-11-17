using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class TestDLLScript
{
    [DllImport("TestDLL")]
    public static extern int InitParticle2D();
    [DllImport("TestDLL")]
    public static extern int DoParticle2D();
    [DllImport("TestDLL")]
    public static extern int TermFoo();
}

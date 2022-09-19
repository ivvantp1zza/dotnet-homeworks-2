.class public abstract sealed auto ansi beforefieldinit
  Hw2.Calculator
{
    .method static public float64 Calculate(float64 arg1, valuetype Hw2.CalculatorOperation operation, float64 arg2) cil managed
    {
        .maxstack 2
        ldarg.1
        switch (IL_0001, IL_0002, IL_0003, IL_0004)
        br.s IL_0005
        
        IL_0001: ldarg.0
        ldarg.2
        add
        ret
        
        IL_0002: ldarg.0
        ldarg.2
        sub
        ret
        
        IL_0003: ldarg.0
        ldarg.2
        mul
        ret
        
        IL_0004: ldarg.0
        ldarg.2
        div
        ret
        
        IL_0005: newobj instance void [System.Runtime]System.ArgumentOutOfRangeException::.ctor()
        throw 
    }
}



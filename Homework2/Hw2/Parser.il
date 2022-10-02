.class public abstract sealed auto ansi beforefieldinit
  Hw2.Parser
{
    .method public static void ParseCalcArguments(string[] args, 
                                                 [out] float64& val1, 
                                                 [out] valuetype Hw2.CalculatorOperation& operation, 
                                                 [out] float64& val2) cil managed
    {
        .maxstack 5
        ldarg.0
        call bool Hw2.Parser::IsArgLengthSupported(string[])
        brfalse.s IL_0001
        
        ldarg.0
        ldc.i4.0
        ldelem.ref
        ldarg.1
        call bool [System.Runtime]System.Double::TryParse(string, float64&)
        brfalse.s IL_0001
        
        ldarg.0
        ldc.i4.2
        ldelem.ref
        ldarg.3
        call bool [System.Runtime]System.Double::TryParse(string, float64&)
        brfalse.s IL_0001
        
        ldarg.2
        ldarg.0
        ldc.i4.1
        ldelem.ref
        call valuetype Hw2.CalculatorOperation Hw2.Parser::ParseOperation(string)
        stind.i4
        ldarg.2
        ldind.i4
        ldc.i4.4
        beq.s IL_0002
        ret
        
        
        IL_0001: newobj instance void [System.Runtime]System.ArgumentException::.ctor()
        throw
        
        
        IL_0002: newobj instance void [System.Runtime]System.InvalidOperationException::.ctor()
        throw
    }
    
    .method private static bool IsArgLengthSupported(string[] args) cil managed
    {   
        .maxstack 2
        ldarg.0
        ldlen
        ldc.i4.3
        ceq
        ret
    }
    
    .method private static valuetype Hw2.CalculatorOperation ParseOperation(string) cil managed
    { 
        .locals init ([0] valuetype Hw2.CalculatorOperation oper0)
        .maxstack 2
        ldarg.0
        ldstr "+"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue.s IL_0001
        
        ldarg.0
        ldstr "-"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue.s IL_0002
        
        ldarg.0
        ldstr "*"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue.s IL_0003
        
        ldarg.0
        ldstr "/"
        call bool [System.Runtime]System.String::op_Equality(string, string)
        brtrue.s IL_0004
        
        br.s IL_0005
        
        IL_0001: ldc.i4.0
        br.s IL_0006
        
        IL_0002: ldc.i4.1
        br.s IL_0006
        
        IL_0003: ldc.i4.2
        br.s IL_0006
        
        IL_0004: ldc.i4.3
        br.s IL_0006
        
        IL_0005: ldc.i4.4
        br.s IL_0006
        
        
        IL_0006: stloc.0
        ldloc.0
        ret
    }
}

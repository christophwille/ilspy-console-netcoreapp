# ilspy-console-netcoreapp
netcoreapp 2.0 demo console application using icsharpcode.decompiler nuget

```
./ilspycmd -h

Usage:  [arguments] [options]

Arguments:
  Assembly filename name  The assembly that is being decompiled. This argument is mandatory.

Options:
  -h|--help                   Show help information
  -p|--project                Decompile assembly as compilable project. This requires the output directory option.
  -o|--outputdir <directory>  The output directory, if omitted decompiler output is written to standard out.
  -t|--type <type-name>       The FQN of the type to decompile.
  -l|--list <entity-type(s)>  Lists all entities of the specified type(s). Valid types: c(lass), i(interface), s(truct),
 d(elegate), e(num)

-o is valid with every option and required when using -p.
```

![dotnet-build-dance](Running.gif)

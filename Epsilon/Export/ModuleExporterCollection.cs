﻿using Epsilon.Abstractions.Export;
using Epsilon.Export.Exceptions;

namespace Epsilon.Export;

public class ModuleExporterCollection : IModuleExporterCollection
{
    private readonly IEnumerable<ICanvasModuleExporter> _exporters;

    public ModuleExporterCollection(IEnumerable<ICanvasModuleExporter> exporters)
    {
        _exporters = exporters;
    }

    public IEnumerable<string> Formats()
    {
        return _exporters.SelectMany(static x => x.Formats);
    }

    public IDictionary<string, ICanvasModuleExporter> DetermineExporters(IEnumerable<string> formats)
    {
        var formatsArray = formats.ToArray(); // To prevent multiple enumeration
        var foundExporters = new Dictionary<string, ICanvasModuleExporter>();

        foreach (var exporter in _exporters)
        {
            foreach (var format in formatsArray.Where(f => exporter.Formats.Contains(f.ToUpperInvariant())))
            {
                foundExporters.Add(format, exporter);
            }
        }

        return !foundExporters.Any()
            ? throw new NoExportersFoundException(formatsArray)
            : (IDictionary<string, ICanvasModuleExporter>)foundExporters;
    }
}
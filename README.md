# Bev.Instruments.Thorlabs.Ccs

Lightweight .NET adapter and helpers for Thorlabs CCS family array spectrometers.

Provides:
- A small, testable API surface for instrument metadata and acquisition (`ThorlabsCcs`, `IArraySpectrometer`).
- Helpers for computing an optimal integration time.

Author: Michael Matus  
Target framework: .NET Framework 4.7.2  
Language: C# 7.3

---

## Requirements

- Windows with Thorlabs CCS drivers installed and the device connected.
- The native/interop assembly `Thorlabs.ccs.interop64` (used by the project).
- Visual Studio 2022 (or MSBuild) to build the project.

---

## Quick start

1. Open the solution in Visual Studio.
   - Build: __Build > Build Solution__
2. Create and use a `ThorlabsCcs` instance:

---

## API highlights

- `ThorlabsCcs` (implements `IArraySpectrometer`)
  - Constructors:
    - `ThorlabsCcs(ProductID id, string serialNumber)`
    - `ThorlabsCcs(string serialNumber)` (defaults to `CCS100`)
  - Properties: `InstrumentManufacturer`, `InstrumentType`, `InstrumentSerialNumber`, `InstrumentFirmwareVersion`, `InstrumentDriverRevision`, `Wavelengths`, `MinimumWavelength`, `MaximumWavelength`, `ResourceName`
  - Acquisition: `GetScanData()`, `GetRawScanData()`
  - Integration time: `SetIntegrationTime(double seconds)`, `GetIntegrationTime()`
  - Utility: `GetOptimalIntegrationTime()` (searches for an integration time to reach a target signal)

- `IArraySpectrometer` — lightweight interface for the spectrometer surface, useful for testing and abstraction.

---

## Notes & recommendations

- This library directly depends on the Thorlabs interop DLL. When publishing a NuGet package or distributing binaries, document the native dependency and driver installation steps.
- Follow SRP: many instrument algorithms (exposure search, averaging, higher-level processing) are implemented as partial classes or helpers. Consider extracting acquisition and measurement algorithms into dedicated services for easier testing and mocking.
- The library targets .NET Framework 4.7.2 — if you need .NET Core/.NET 5+ support, the native interop and threading model will need review.

---

## Contributing

Contributions are welcome. Please:
1. Open an issue to discuss significant changes.
2. Create a feature branch, commit with clear messages, and open a pull request.


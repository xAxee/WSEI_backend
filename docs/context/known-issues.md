# Known Issues

## Code issues

- Encje w `AppCore/Entities` mają kilka właściwości nienullowalnych bez wartości domyślnych, co generuje ostrzeżenia nullability podczas `dotnet build`.
- `MemoryVehicleRepository.FindByNumberPlate` może zwracać `null`, ale sygnatura metody nadal jest nienullowalna.

## Environment issues

- W tym kroku nie wystąpił błąd środowiskowy blokujący implementację.
- Nadal obowiązuje znane ograniczenie ścieżki roboczej z `!`, które może wpływać na niektóre narzędzia frontendowe/testowe na Windows.

## Missing external setup

- Brak konfiguracji bazy danych i migracji — obecny krok nie wymagał zmian schematu.
- Brak docelowych endpointów API dla domeny parkingu.
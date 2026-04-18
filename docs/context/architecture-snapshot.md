# Architecture Snapshot

## Projekty

- `AppCore` — encje domenowe, DTO i kontrakty repozytoriów.
- `Infrastructure` — implementacje repozytoriów in-memory.
- `WebApp` — bootstrap ASP.NET Core Minimal API.
- `UnitTest` — testy jednostkowe.

## Repozytoria in-memory

- `MemoryGenericRepository<T>` przechowuje encje w chronionym słowniku `_data` i obsługuje podstawowy CRUD oraz paginację.
- Repozytoria konkretne są umieszczone w `Infrastructure/Memory`:
  - `MemoryVehicleRepository`
  - `MemoryParkingGateRepository`
  - `MemoryParkingSessionRepository`
- Każde repozytorium konkretne inicjalizuje przykładowe rekordy w konstruktorze i realizuje własne metody wyszukiwania/filtrowania na `_data`.

## Mapowanie obiektów

- Mapowanie zostało osadzone bezpośrednio w rekordach DTO.
- Wzorzec:
  - `FromEntity(...)` dla mapowania encja → DTO,
  - `ToEntity(...)` dla DTO wejściowych i DTO, które mają sensowny odpowiednik encji,
  - `FromEntities(...)` dla DTO agregacyjnych, np. statystyk.
- Konwersje `string -> enum` są walidowane przez `Enum.TryParse(..., ignoreCase: true)`.

## Ograniczenia obecnego stanu

- Brak warstwy application/service.
- Brak właściwych endpointów parkingowych.
- Brak rejestracji repozytoriów w DI, bo `WebApp` nadal nie używa jeszcze logiki domenowej parkingu.
- Brak trwałej persystencji (na razie pamięć procesu).
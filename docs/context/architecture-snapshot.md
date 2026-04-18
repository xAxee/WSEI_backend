# Architecture Snapshot

## Projekty

- `AppCore` — encje domenowe, DTO i kontrakty repozytoriów.
- `Infrastructure` — implementacje repozytoriów in-memory.
- `WebApp` — bootstrap ASP.NET Core Minimal API.
- `UnitTest` — testy jednostkowe.

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
- Brak trwałej persystencji (na razie pamięć procesu).
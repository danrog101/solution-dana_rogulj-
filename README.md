# Katalog proizvoda — Fullstack ulazni zadatak

Fullstack aplikacija za pregled kataloga proizvoda s pretragom, filtriranjem i
prikazom detalja. Sastoji se od backend REST API-ja (ASP.NET Web API) koji
dohvaća podatke iz vanjskog izvora (DummyJSON) i frontend SPA aplikacije
(React + TypeScript) koja komunicira isključivo s vlastitim backendom.

Tok podataka: **frontend → backend → DummyJSON**

## Struktura repozitorija

```
backend/ProductCatalog.Api/   ASP.NET Web API (.NET 9)
frontend/                     React + TypeScript (Vite)
```

## Preduvjeti

- .NET SDK 8 ili noviji — https://dotnet.microsoft.com/download
- Node.js 18 ili noviji — https://nodejs.org

## Pokretanje backenda

```
cd backend/ProductCatalog.Api
dotnet run
```

Backend se pokreće na `http://localhost:5262`.
Swagger dokumentacija endpointova: `http://localhost:5262/swagger`

Konfiguracija je u `appsettings.json`:

| Ključ                   | Opis                        | Zadana vrijednost       |
|-------------------------|-----------------------------|-------------------------|
| `ProductSource:BaseUrl` | Adresa vanjskog izvora      | `https://dummyjson.com/`|

## Pokretanje frontenda

```
cd frontend
npm install
npm run dev
```

Frontend se pokreće na `http://localhost:5173`.

Konfiguracija je u `frontend/.env`:

| Varijabla      | Opis                    | Zadana vrijednost           |
|----------------|-------------------------|-----------------------------|
| `VITE_API_URL` | Bazna adresa backend API-ja | `http://localhost:5262/api` |

Za rad aplikacije oba servera moraju biti pokrenuta istovremeno
(backend u jednom terminalu, frontend u drugom).

## Backend endpointovi

Svi endpointovi su dokumentirani i isprobivi kroz Swagger. Sažetak:

| Metoda | Adresa                     | Opis |
|--------|----------------------------|------|
| GET    | `/api/products`            | Lista proizvoda (slika, naziv, cijena, opis do 100 znakova) s paginacijom, pretragom i filtriranjem |
| GET    | `/api/products/{id}`       | Puni detalji jednog proizvoda (404 ako ne postoji) |
| GET    | `/api/products/categories` | Popis svih kategorija |

Query parametri za `/api/products`:

| Parametar  | Opis                              |
|------------|-----------------------------------|
| `search`   | Pretraga po nazivu                |
| `category` | Filtriranje po kategoriji         |
| `minPrice` | Minimalna cijena                  |
| `maxPrice` | Maksimalna cijena                 |
| `page`     | Broj stranice (zadano 1)          |
| `pageSize` | Veličina stranice (zadano 12)     |

Primjer: `/api/products?search=phone&minPrice=100&maxPrice=500&page=1&pageSize=12`

## Arhitektura backenda

Dohvat proizvoda odvojen je iza apstrakcije `IProductSource` (interface), a
`DummyJsonProductSource` je njegova implementacija za DummyJSON. Controlleri
poznaju samo interface, pa se u budućnosti novi izvor podataka (baza, file
system, drugi web servis) dodaje kao nova implementacija i jedna promjena
registracije u `Program.cs`, bez izmjena ostatka aplikacije.

Filtriranje po rasponu cijene DummyJSON ne podržava, pa backend dohvaća
proizvode i filtrira ih u memoriji. Za ovaj opseg podataka (194 proizvoda) to
je prihvatljivo; kod većeg izvora razmotrila bih caching dohvaćene liste kako
se ponovljeni pozivi pretrage i filtriranja ne bi svaki put prosljeđivali
vanjskom izvoru.

## Funkcionalnosti frontenda

- Lista proizvoda u karticama s paginacijom
- Stanja sučelja: učitavanje, prazan rezultat i greška
- Ruta `/products/:id` s punim detaljima (galerija, opis, ocjena, kategorija, zaliha)
- Pretraga po nazivu s debounceom unosa (400 ms)
- Filtriranje po kategoriji i rasponu cijene
- Svi filteri i pretraga zapisani u URL-u (query parametri) — stanje preživi
  osvježavanje stranice i može se dijeliti linkom
- Povratak s detalja na listu uz očuvanje filtera i stranice
- Mobile-first responzivni layout (1 / 2 / 4 kolone)

## Korištenje AI alata

Tijekom izrade zadatka koristila sam Claude (Anthropic) kao mentora:

- postavljanje i struktura ASP.NET Web API projekta te objašnjenje koncepata
  (dependency injection, apstrakcija izvora kroz interface, async/await)
- dijagnoza greške pri pokretanju: sukob verzija paketa `Microsoft.OpenApi`
  između .NET 9 predloška (`Microsoft.AspNetCore.OpenApi`) i Swashbucklea —
  riješeno uklanjanjem nekorištenog paketa
- vođenje kroz implementaciju frontenda (React Router, stanje filtera u URL-u
  kroz `useSearchParams`, debounce pretrage)

 Primjere promptova
prilažem u `docs/ai-usage.md`.
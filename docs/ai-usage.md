# Korištenje AI alata — primjeri promptova

Zadatak sam radila uz Claude (Anthropic) kao mentora — vodio me korak po
korak, objašnjavao koncepte i pomagao kod
grešaka. Kod sam djelomično pisala uz upute i tražila objašnjenja svake cjeline kako
bih razumjela arhitekturu i mogla je obraniti. U nastavku su primjeri
stvarnih promptova iz razgovora.



## Debuggiranje greške pri pokretanju backenda

Nakon dodavanja Swashbucklea aplikacija se rušila pri pokretanju. Zalijepila
sam cijeli ispis greške iz terminala:

> Unhandled exception. System.Reflection.ReflectionTypeLoadException:
> Unable to load one or more of the requested types.
> Could not load type 'Microsoft.OpenApi.Any.IOpenApiAny' from assembly
> 'Microsoft.OpenApi, Version=2.7.5.0' ...

Dijagnoza: sukob verzija biblioteke `Microsoft.OpenApi` između paketa
`Microsoft.AspNetCore.OpenApi` (dolazi s .NET 9 predloškom) i Swashbucklea.
Rješenje: uklanjanje nekorištenog paketa
(`dotnet remove package Microsoft.AspNetCore.OpenApi`).

## Prilagodba koda mojoj razini znanja

Tražila sam da kod bude pisan tako da ga u potpunosti razumijem — bez LINQ
lančanja i skraćenih sintaksi, s običnim petljama i eksplicitnim tipovima:

> treba mi puno jednostavniji kod bez gotovih funkcija no isnullorwhitespace
> hasvalue where skip take slecet to list ... radis prekomplicirano a ja sam
> student trece godine i zelim razumijeti sam kod

## Propitivanje koda i alternativa

Kad bih naišla na sintaksu koju ne razumijem, pitala sam može li drugačije —
i obrnuto.

> a jesmo li mogli koristiti mozda ovo da nam bude jednostavnije? tj manji
> kod? jesan li na dobrom putu
> WithOrigins(frontendUrl).AllowAnyHeader().AllowAnyMethod()

## Učenje i razumijevanje umjesto kopiranja

Nakon svake veće cjeline tražila sam detaljna objašnjenja svega napisanog i korištenog
(dependency injection, apstrakcija kroz interface,
debounce, stanje u URL-u):

> ok sad stajemo na tom koraku i sad ces mi detaljno napisati sta sam
> koristila za sta, odnosno nauci me detaljno da ja znam za iduci put to
> napraviti



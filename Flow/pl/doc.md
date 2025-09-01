# Flow – Uživatelská příručka

## Obsah

1. [Úvod](#Úvod)
2. [Základní syntaxe](#Základní syntaxe)
3. [Proměnné](#Proměnné)
4. [Výrazy](#výrazy)
5. [Výpis na obrazovku](#výpis-na-obrazovku)
6. [Komentáře](#komentáře)
7. [Chybové hlášky](#chybové-hlášky)
8. [Příklady](#příklady)

---

## Úvod

Flow je homemade skriptovací jazyk napsaný c#

---

## Základní syntaxe

- Každý příkaz **musí končit středníkem** `;`.
- Jazyk je **case-sensitive** (rozlišuje velká a malá písmena).

---

## Typy
### Bool
  
- může být 
```true``` nebo 
```false```
- používa se při while, if atd.

### Int a float

- může mýt číselnou hodnotu
- float může být přetypován na int a a naopak
### String

- zapisuje se do ůvozovek: 
```"string"```
- pokud je číslo a prvadí se s ním matematická operace bude s ním nakládáno jako s číslem

## Proměnné

Proměnné deklaruješ pomocí klíčového slova `var`. Každá proměnná musí mít přiřazenou hodnotu.

### Deklarace

```flow
let jmeno_promenne = výraz;
```
```jmeno_promenne```: název proměnné (může obsahovat písmena, číslice a podtržítka, nesmí začínat číslem)

```výraz```: hodnota podporovaného typu např bool, int nebo float

Příklad
```
let x = 10;
let y = 20;
let suma = x + y;
```
Výrazy
Podporované operace pro float a int jsou:

- Plus:a+b
- Minus:a-b
- Násobení:a*b
- Dělení:a/b

Závorky
Můžeš používat závorky ( a ) pro určení priority operací:

```
let result = (10 + 5) * 2;
```

---
## Výpis
Výpis na obrazovku
Pro zobrazení hodnot použij příkaz println.

Syntaxe
```println(výraz);```
```výraz```: může být číslo, proměnná, nebo aritmetický výraz

Příklad
```
let x = 42;
println(x);
println(x + 8);
```
Výstupem bude: 

42

50

---
## Vstup  od uživatele
Funkce```input()``` aktuálně vrací pouze int.
příklad:
```
let i  = 50;
let in = input();
let solution=i*in;
println(solution);
```

---
## Komentáře

```#Toto je koment``` 
---
### Chybové hlášky
```Syntax error: expected ... found ...``` — znamená, že ve zdrojovém kódu je nějaká nesprávná syntaxe (např. chybí středník nebo špatně napsaný příkaz)

```Invalid value in variable assignment.``` — přiřazuješ proměnné něco, co není číslo nebo výraz, který program neumí vyhodnotit

```Division by zero``` — pokus o dělení nulou

Jiná chybová hláška ti přesně řekne, co je špatně — vždy pečlivě kontroluj, kde jsi v kódu udělal chybu.

## Kompletní příklady
#### Příklad 1 – základní aritmetika a výpis
```
let a = 5;
let b = 3;
let c = a * (b + 2);
println(c);
```
Výstup:

```25```
#### Příklad 2 – více proměnných a součet
```
var x = 10;
var y = 7;
var sum = x + y;
println(sum);
```
Výstup:


```17```
#### Příklad 3 – chybný kód (chybná syntaxe)
```
var x = 10
println(x);
```
Chyba:

```Syntax error: expected Semicolon found:EOF```

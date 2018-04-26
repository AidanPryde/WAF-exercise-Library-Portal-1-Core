# WAF-exercise-Library-Portal-1-ASP.NET-CWA
The Web Application Development course exercise at Hungarian university ELTE IK.

Online adatkezelő rendszer

Közös követelmények:

* A rendszert négy komponensből építjük fel, amelyek a következők:

  * adatbázis, amely tartalmazza a rendszer által használt adatokat (beleértve a felhasználói információkat);
  * webes felhasználói felület ASP.NETCore-ban, MVC architektúrával, amely a felhasználói funkciókat biztosítja, és Entity Framework segítségével kapcsolódik az adatbázishoz;
  * web szolgáltatás ASP.NETCore-ban, MVC architektúrával, amely az adminisztratív funkciókat biztosítja, és Entity Frameworksegítségével kapcsolódik az adatbázishoz;
  * adminisztratív kliens, WPF grafikus felülettel, MVVM architektúrában, amely az adminisztrátori funkciókat biztosítja, és a web szolgáltatáshoz csatlakozik.
	
* A rendszer felépítésében ügyelni kell arra, hogy a kliensek tetszőleges számban csatlakozhatnak a szerverre és szerkeszthetik az adatokat. Feltételezhető, hogy ugyanaz a felhasználó nem jelentkezik be szimultán több kliensen.

* A klienseknek biztosítania kell a megadott adatok megjelenítését és szerkesztését, adott feladatoknál a felhasználói authentikációt (bejelentkezés, kijelentkezés, regisztráció). Az adatok bevitelénél törekedni a felhasználóbarát, hibamenetes megoldásokra. Ahol lehetséges, biztosítsuk a kiválasztási lehetőséget, adatbevitelnél ellenőrizni kell az adatok helyességét (pl. az évszám csak megfelelő intervallumban lévő egész szám lehet, a telefonszámban csak számjegy és elválasztó karakter szerepelhet, irányítószám 4 számjegyű lehet, a személyi igazolvány szám hat számból és két betűből áll). A program ne fogadjon el hibás adatot, illetve ne omoljon össze bármilyen hibás adat megadása esetén.

* Törekedni kell az adatok biztonságos kezelésére, illetve tárolására is, pl. jelszavak titkosított tárolása az adatbázisban. Amennyiben a feladat felhasználói authentikációt is elvár, azt a szerveren keresztül hajtsuk végre, és csak azt követően engedélyezzük hozzáférést az adatokhoz.

* Az adatbázist megfelelő számú mintaadattal kell ellátni, amely elősegíti a tesztelést.

* A web szolgáltatás funkcionalitását megfelelőszámú egységteszttel (unit test) kell ellenőrizni. Minden funkcióhoz 2-3 tesztesetet kell készíteni.

* A dokumentációnak tartalmaznia kell a feladat elemzését, felhasználói eseteit (UML felhasználói esetek diagrammal), a rendszerszerkezetének leírását (UML komponens, valamint osztály diagrammal), az adatbázis felépítésének leírását (egyedkapcsolati diagrammal), a web szolgáltatás felületének leírását, valamint a web szolgáltatás teszteseteinek leírását.

(7.) Könyvtári kölcsönző

Készítsük el egy könyvtár online kölcsönzői és nyilvántartó rendszerét, amellyel a látogatók kölcsönzését, előjegyzését, valamint a könyvtárosok adminisztratív munkáját tudjuk támogatni.

(1.) részfeladat: a webes felületen a látogatók adhatnak le online előjegyzéseket, illetve böngészhetik a kínálatot.

* A főoldalon a könyvtárban elérhető könyvek kerülnek ki listázásra, alapértelmezetten népszerűségi sorrendben (kölcsönzések száma), de a felhasználó választhat cím szerinti lexikografikus rendezést is. Egy oldalon legfeljebb 20 könyv jelenik meg, a többit lapozással lehet elérni. A könyvek címmel, (első) szerzővel, kiadási évvel, ISBN számmal és borítóképpel rendelkeznek, amelyek megjelenítésre kerülnek.

* A látogatók a könyvek között cím és szerző szerint, szabad szavas beviteli mező segítségével szűrhetnek.

* Egy könyvet kiválasztva megjelenítésre kerülnek a kötetei, valamint esetlegesen aktív kölcsönzésük és jövőbeni előjegyzéseik.

* A látogatók regisztrációt (név, telefonszám, e-mail cím, felhasználónév, jelszó, megerősített jelszó) és bejelentkezést követően előjegyzést adhatnak le egy kötetre, a kölcsönzés tervezett kezdő és befejező napját megadva. Az előjegyzés nem fedhet át aktív kölcsönzéssel vagy más előjegyzéssel.

Az adatbázis az alábbi adatokat tárolja:
* könyvek (cím, szerző, kiadás éve, ISBN szám, borítókép);
* kötetek (könyv, könyvtári azonosító);
* kölcsönzések (kötet, látogató, kezdő nap, befejező nap, aktív-e)
* látogatók (név, cím, telefonszám, azonosító, jelszó);
* könyvtárosok (név, azonosító, jelszó).

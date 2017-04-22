Dungeon levels are stored in `[SARC](file_formats/sarc.md)` archive files in
the Packs directory, files are named `Dungeonxxx.pack` from `000` to `119`
numerically.

The extracted directory hierarchy tree structure on average follows this:

```
* `Actor`
  * `Pack`
    * `DgnMrgPrt_Dungeon000.sbactorpack`
* `Map`
  * `CDungeon`
    * `Dungeon000`
      * `Dungeon000_Clustering.sblwp`
      * `Dungeon000_Dynamic.smubin`
      * `Dungeon000_Static.smubin`
      * `Dungeon000_TeraTree.sblwp`
  * `DungeonData`
    * `CDungeon`
      * `Dungeon000.bdgnenv`
* `Model`
  * `DgnMrgPrt_Dungeon000.sbfres`
  * `DgnMrgPrt_Dungeon000.Tex2.sbfres`
* `NavMash`
  * `CDungeon`
    * `Dungeon000`
      * `Dungeon000.shknm2`
* `Physics`
  * `StaticCompound`
    * `CDungeon`
      * `Dungeon000.shksc`

**Check 10000 files to read**

*Min size (bytes): 3 bytes, max size (bytes): 54989002, average size (bytes): 210100,5686*

| Scenario | Time | IO Read KBytes/sec | Current Disk Queue Length | Page Faults Faults/sec | Disk Queue Length Count | Total .Net Memory Mb | Concurrent Threads  | Current lock Queue Length | Available MBytes Mb | Disk Read Kb / sec | Disk Read Time  | CPU Load % | Threads Count | Was failed |
| -------- | -------- | -------- | -------- | -------- |
| ScenarioAsyncWithMaxParallelCount4 | 00:00:00 |  | True |
| ScenarioAsyncWithMaxParallelCount8 | 00:00:00 |  | True |
| ScenarioAsyncWithMaxParallelCount16 | 00:00:00 |  | True |
| ScenarioAsyncWithMaxParallelCount32 | 00:00:00 |  | True |
| ScenarioAsyncWithMaxParallelCount64 | 00:00:00 |  | True |
| ScenarioAsyncWithMaxParallelCount128 | 00:00:00 |  | True |
| ScenarioAsyncWithMaxParallelCount256 | 00:00:00 |  | True |
| ScenarioSyncAsParallel | 00:00:00 |  | True |
| ScenarioReadAllAsParallel | 00:00:00 |  | True |
| ScenarioAsync | 00:00:00 |  | True |
| ScenarioAsync2 | 00:00:00 |  | True |
| ScenarioNewThread | 00:00:00 |  | True |

*Min size (bytes): 1001 bytes, max size (bytes): 54989002, average size (bytes): 210818,0652*

| Scenario | Time | IO Read KBytes/sec | Current Disk Queue Length | Page Faults Faults/sec | Disk Queue Length Count | Total .Net Memory Mb | Concurrent Threads  | Current lock Queue Length | Available MBytes Mb | Disk Read Kb / sec | Disk Read Time  | CPU Load % | Threads Count | Was failed |
| -------- | -------- | -------- | -------- | -------- |
| ScenarioAsyncWithMaxParallelCount4 | 00:00:00.6350000 | 1390949,69 | 4,48 | 516653,52 | 389803,98 | 138,61 | 12,54 | 1,00 | 2216,70 | 560768710,78 | 7571423736983,14 | 65707692,31 | 20,44 | False |
| ScenarioAsyncWithMaxParallelCount8 | 00:00:00.6500000 | 1202907,27 | 4,20 | 389057,24 | 390914,26 | 176,80 | 11,39 | 1,00 | 2187,70 | 562669706,71 | 7575131193908,19 | 47130627,39 | 19,37 | False |
| ScenarioAsyncWithMaxParallelCount16 | 00:00:00.7170000 | 1279682,89 | 5,36 | 395288,07 | 391605,27 | 232,21 | 15,55 | 1,00 | 2099,18 | 564861039,20 | 7585603443202,30 | 54608525,51 | 23,49 | False |
| ScenarioAsyncWithMaxParallelCount32 | 00:00:00.5380000 | 1228625,11 | 14,90 | 301908,95 | 392321,89 | 280,47 | 26,17 | 1,00 | 1957,84 | 567963651,17 | 7619259602686,62 | 69248134,16 | 34,03 | False |
| ScenarioAsyncWithMaxParallelCount64 | 00:00:00.7150000 | 1334137,94 | 9,05 | 404071,79 | 393718,00 | 183,87 | 16,35 | 1,00 | 2139,40 | 570589156,94 | 7646415428810,83 | 56327172,26 | 24,31 | False |
| ScenarioAsyncWithMaxParallelCount128 | 00:00:00.5960000 | 1270762,72 | 3,05 | 417147,10 | 394675,91 | 198,92 | 11,52 | 1,00 | 2159,60 | 572294429,48 | 7661237504146,27 | 52425781,25 | 19,65 | False |
| ScenarioAsyncWithMaxParallelCount256 | 00:00:00.5810000 | 1346905,00 | 1,86 | 464545,94 | 395051,87 | 157,04 | 10,22 | 1,00 | 2187,75 | 573375238,99 | 7665134237487,44 | 60385243,06 | 18,22 | False |
| ScenarioSyncAsParallel | 00:00:00.4150000 | 1053776,20 | 0,00 | 430225,43 | 395195,20 | 40,45 | 7,00 | 0,30 | 2216,03 | 574028356,45 | 7667309795677,75 | 47625000,00 | 14,85 | False |
| ScenarioReadAllAsParallel | 00:00:00.5990000 | 920564,10 | 0,00 | 146981,90 | 395222,00 | 40,30 | 7,00 | 0,00 | 2081,70 | 574104295,00 | 7667378226090,00 | 23343750,00 | 15,00 | False |
| ScenarioAsync | 00:00:00.3650000 | 1464366,47 | 14,94 | 691823,75 | 396296,67 | 384,82 | 25,42 | 0,91 | 1985,91 | 576405032,36 | 7699184859183,88 | 84311263,10 | 33,36 | False |
| ScenarioAsync2 | 00:00:00.5950000 | 1214141,73 | 16,53 | 594096,10 | 398989,81 | 928,81 | 26,74 | 0,80 | 1554,56 | 584096569,96 | 7789528415040,33 | 103073537,79 | 33,49 | False |
| ScenarioNewThread | 00:00:00.6660000 | 1547847,12 | 23,73 | 477964,04 | 401484,78 | 246,88 | 50,78 | 0,68 | 1781,34 | 592012435,45 | 7902389637149,96 | 137352452,54 | 46,02 | False |

*Min size (bytes): 10008 bytes, max size (bytes): 135657872, average size (bytes): 421055,565*

| Scenario | Time | IO Read KBytes/sec | Current Disk Queue Length | Page Faults Faults/sec | Disk Queue Length Count | Total .Net Memory Mb | Concurrent Threads  | Current lock Queue Length | Available MBytes Mb | Disk Read Kb / sec | Disk Read Time  | CPU Load % | Threads Count | Was failed |
| -------- | -------- | -------- | -------- | -------- |
| ScenarioAsyncWithMaxParallelCount4 | 00:00:00.2280000 | 2286163,45 | 4,75 | 792446,39 | 421869,79 | 116,88 | 14,84 | 1,00 | 2391,29 | 609840815,83 | 8013897981362,61 | 195726522,19 | 20,28 | False |
| ScenarioAsyncWithMaxParallelCount8 | 00:00:00.4770000 | 2248281,64 | 7,35 | 729369,93 | 451558,76 | 143,38 | 19,33 | 1,00 | 2351,31 | 636627943,56 | 8119762954634,72 | 247038767,51 | 24,56 | False |
| ScenarioAsyncWithMaxParallelCount16 | 00:00:00.5970000 | 2229082,38 | 13,55 | 671606,86 | 484978,24 | 189,16 | 26,48 | 1,00 | 2269,98 | 663396712,10 | 8294416011757,20 | 250439169,87 | 31,80 | False |
| ScenarioAsyncWithMaxParallelCount32 | 00:00:00.5060000 | 2189273,57 | 20,64 | 627586,74 | 522116,57 | 245,60 | 35,71 | 1,00 | 2085,93 | 690223471,35 | 8605456140363,62 | 212335215,47 | 40,82 | False |
| ScenarioAsyncWithMaxParallelCount64 | 00:00:00 |  | True |
| ScenarioAsyncWithMaxParallelCount128 | 00:00:00 |  | True |
| ScenarioAsyncWithMaxParallelCount256 | 00:00:00 |  | True |
| ScenarioSyncAsParallel | 00:00:00 |  | True |
| ScenarioReadAllAsParallel | 00:00:00 |  | True |
| ScenarioAsync | 00:00:00 |  | True |
| ScenarioAsync2 | 00:00:00 |  | True |
| ScenarioNewThread | 00:00:00 |  | True |

*Min size (bytes): 100072 bytes, max size (bytes): 135657872, average size (bytes): 969049,0894*

| Scenario | Time | IO Read KBytes/sec | Current Disk Queue Length | Page Faults Faults/sec | Disk Queue Length Count | Total .Net Memory Mb | Concurrent Threads  | Current lock Queue Length | Available MBytes Mb | Disk Read Kb / sec | Disk Read Time  | CPU Load % | Threads Count | Was failed |
| -------- | -------- | -------- | -------- | -------- |
| ScenarioAsyncWithMaxParallelCount4 | 00:00:00.6880000 | 5118594,84 | 17,51 | 1634681,20 | 661683,54 | 112,75 | 15,21 | 1,00 | 283,24 | 828435400,86 | 11621444636086,00 | 356886473,25 | 20,23 | False |
| ScenarioAsyncWithMaxParallelCount8 | 00:00:00.2160000 | 5215954,47 | 20,50 | 1521889,30 | 692707,94 | 211,95 | 20,33 | 1,00 | 312,41 | 881733878,16 | 12148682611901,80 | 493169656,96 | 25,01 | False |
| ScenarioAsyncWithMaxParallelCount16 | 00:00:00.5870000 | 5172332,26 | 26,04 | 1315011,57 | 727792,68 | 307,84 | 27,94 | 1,00 | 312,31 | 935796510,78 | 12905226560768,00 | 551028475,76 | 32,52 | False |
| ScenarioAsyncWithMaxParallelCount32 | 00:00:00.5700000 | 5173367,93 | 34,98 | 1017879,17 | 765425,82 | 433,46 | 40,73 | 1,00 | 323,07 | 991374071,46 | 14091978944248,80 | 434326562,34 | 45,24 | False |
| ScenarioAsyncWithMaxParallelCount64 | 00:00:00.5070000 | 5191921,66 | 33,06 | 1019868,35 | 804937,82 | 487,91 | 43,79 | 1,00 | 482,89 | 1047985390,16 | 15510653860656,00 | 352345478,81 | 48,26 | False |
| ScenarioAsyncWithMaxParallelCount128 | 00:00:00.4060000 | 5261013,42 | 31,56 | 1000298,42 | 842618,29 | 512,84 | 44,51 | 1,00 | 447,33 | 1104427211,88 | 16884733577544,70 | 350080626,80 | 48,99 | False |
| ScenarioAsyncWithMaxParallelCount256 | 00:00:00.4800000 | 5119501,17 | 33,26 | 919502,60 | 879974,56 | 537,41 | 48,70 | 1,00 | 441,20 | 1161150814,93 | 18337161527987,90 | 349792269,26 | 53,51 | False |
| ScenarioSyncAsParallel | 00:00:00 |  | True |
| ScenarioReadAllAsParallel | 00:00:00.4680000 | 5268351,25 | 7,57 | 799575,16 | 910671,60 | 49,02 | 5,93 | 0,47 | 433,20 | 1214166607,90 | 19082196953771,00 | 116135684,65 | 10,60 | False |
| ScenarioAsync | 00:00:00.3780000 | 5380759,33 | 44,31 | 1134933,54 | 940193,04 | 776,70 | 55,59 | 0,89 | 362,44 | 1264636337,86 | 20195857046943,00 | 342434679,55 | 60,05 | False |
| ScenarioAsync2 | 00:00:00.5390000 | 4965466,51 | 21,11 | 1361337,11 | 974956,31 | 392,93 | 31,11 | 0,32 | 1150,20 | 1317999875,04 | 21616117276805,00 | 383679369,74 | 36,08 | False |
| ScenarioNewThread | 00:00:00.6730000 | 4980582,56 | 48,09 | 1324760,43 | 1006686,16 | 1623,67 | 4859,16 | 0,88 | 789,51 | 1375303628,94 | 24111083504214,60 | 3550813255,07 | 4682,30 | False |

*Min size (bytes): 1000288 bytes, max size (bytes): 238607633, average size (bytes): 3885047,44311888*

| Scenario | Time | IO Read KBytes/sec | Current Disk Queue Length | Page Faults Faults/sec | Disk Queue Length Count | Total .Net Memory Mb | Concurrent Threads  | Current lock Queue Length | Available MBytes Mb | Disk Read Kb / sec | Disk Read Time  | CPU Load % | Threads Count | Was failed |
| -------- | -------- | -------- | -------- | -------- |
| ScenarioAsyncWithMaxParallelCount4 | 00:00:00.7410000 | 4624059,17 | 10,79 | 1656356,97 | 1046341,74 | 177,01 | 14,56 | 1,00 | 1544,81 | 1435306069,36 | 26644552093540,90 | 272295311,16 | 19,64 | False |
| ScenarioAsyncWithMaxParallelCount8 | 00:00:00.3850000 | 4827479,54 | 12,19 | 1611093,61 | 1076564,59 | 310,20 | 20,15 | 1,00 | 1450,05 | 1480447660,56 | 26881582282584,50 | 391613874,38 | 25,42 | False |
| ScenarioAsyncWithMaxParallelCount16 | 00:00:00.5050000 | 4647588,69 | 18,66 | 1466124,55 | 1111150,03 | 418,93 | 27,73 | 1,00 | 1362,06 | 1525704325,40 | 27254261436593,50 | 426198668,94 | 32,61 | False |
| ScenarioAsyncWithMaxParallelCount32 | 00:00:00.4370000 | 4844416,72 | 28,81 | 1428349,92 | 1148350,32 | 581,89 | 39,17 | 1,00 | 1261,17 | 1572560982,84 | 27946569176372,90 | 374367963,15 | 43,39 | False |
| ScenarioAsyncWithMaxParallelCount64 | 00:00:00.3140000 | 4821505,64 | 33,70 | 1381586,23 | 1184231,17 | 703,21 | 40,63 | 1,00 | 1237,10 | 1620401680,18 | 29060331773147,70 | 295250731,02 | 44,99 | False |
| ScenarioAsyncWithMaxParallelCount128 | 00:00:00.3810000 | 4765428,42 | 35,30 | 1387250,06 | 1221144,41 | 761,37 | 42,06 | 1,00 | 1189,98 | 1669839826,64 | 30337380321688,00 | 285324300,93 | 46,61 | False |
| ScenarioAsyncWithMaxParallelCount256 | 00:00:00.5700000 | 4815005,44 | 35,69 | 1155163,63 | 1257864,69 | 740,94 | 43,25 | 1,00 | 1178,08 | 1719000596,65 | 31510043964690,80 | 262990450,18 | 48,39 | False |
| ScenarioSyncAsParallel | 00:00:00 |  | True |
| ScenarioReadAllAsParallel | 00:00:00.6780000 | 4829879,78 | 14,09 | 727543,86 | 1287294,43 | 110,03 | 6,35 | 0,35 | 1564,86 | 1764694688,99 | 32247011413146,60 | 76733927,98 | 12,38 | False |

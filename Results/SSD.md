**Check 10000 files to read**

*Min size (bytes): 2 bytes, max size (bytes): 25720320, average size (bytes): 40953.1175*

| Scenario | Time | IO Read KBytes/sec | Current Disk Queue Length | Page Faults Faults/sec | Disk Queue Length Count | Total .Net Memory Mb | Concurrent Threads  | Current lock Queue Length | Available MBytes Mb | Disk Read Kb / sec | Disk Read Time  | CPU Load % | Threads Count | Was failed |
| -------- | -------- | -------- | -------- | -------- | -------- | -------- | -------- | -------- | -------- | -------- | -------- | -------- | -------- | -------- |
| ScenarioAsyncWithMaxParallelCount4 | 00:00:00.9580000 | 148910.00 | 0.00 | 74696.00 | 0.00 | 52.60 | 17.30 | 1.00 | 5573.70 | 29681177.80 | 0.00 | 12698481.40 | 20.50 | False |
| ScenarioAsyncWithMaxParallelCount8 | 00:00:00.8360000 | 158522.00 | 0.00 | 63383.60 | 0.00 | 144.20 | 27.60 | 1.00 | 5541.20 | 29682546.00 | 0.00 | 14539293.20 | 31.00 | False |
| ScenarioAsyncWithMaxParallelCount16 | 00:00:00.8160000 | 174582.30 | 0.00 | 66243.60 | 0.00 | 101.13 | 27.80 | 1.00 | 5576.53 | 29682957.00 | 0.00 | 29525789.27 | 31.00 | False |
| ScenarioAsyncWithMaxParallelCount32 | 00:00:00.1090000 | 143361.20 | 0.00 | 64935.00 | 0.00 | 120.30 | 27.80 | 1.00 | 5623.10 | 29683027.60 | 0.00 | 33353013.80 | 31.00 | False |
| ScenarioAsyncWithMaxParallelCount64 | 00:00:00.4410000 | 217256.30 | 0.00 | 105015.60 | 0.00 | 67.20 | 35.20 | 1.00 | 5665.30 | 29683581.60 | 0.00 | 82742930.40 | 38.60 | False |
| ScenarioAsyncWithMaxParallelCount128 | 00:00:00.6090000 | 210598.80 | 0.00 | 133003.70 | 0.00 | 75.40 | 37.40 | 1.00 | 5678.20 | 29691915.90 | 0.00 | 69919648.20 | 40.50 | False |
| ScenarioAsyncWithMaxParallelCount256 | 00:00:00.7090000 | 212871.40 | 0.00 | 153410.70 | 0.00 | 118.00 | 35.40 | 1.00 | 5633.00 | 29693719.30 | 0.00 | 62213198.80 | 38.50 | False |
| ScenarioSyncAsParallel | 00:00:00.8640000 | 91800.40 | 0.00 | 34923.80 | 0.00 | 10.60 | 27.00 | 0.00 | 5729.80 | 29695265.60 | 0.00 | 22027341.20 | 30.00 | False |
| ScenarioReadAllAsParallel | 00:00:00.3270000 | 277756.00 | 0.00 | 54791.00 | 0.00 | 60.60 | 26.80 | 0.00 | 5554.20 | 29696785.00 | 0.00 | 22370543.40 | 30.00 | False |
| ScenarioAsync | 00:00:00.8660000 | 97155.40 | 0.00 | 201298.10 | 0.00 | 679.20 | 27.60 | 0.50 | 5178.40 | 29697244.00 | 0.00 | 76549690.70 | 30.90 | False |
| ScenarioAsync2 | 00:00:00.0710000 | 189345.80 | 0.00 | 82791.40 | 0.00 | 145.80 | 28.00 | 0.00 | 5613.20 | 29697844.60 | 0.00 | 17784114.00 | 31.00 | False |
| ScenarioNewThread | 00:00:00.6560000 | 164768.20 | 0.00 | 87892.40 | 0.00 | 18.40 | 140.80 | 0.00 | 5747.80 | 29698490.70 | 0.00 | 152771779.30 | 85.80 | False |

*Min size (bytes): 1001 bytes, max size (bytes): 25720320, average size (bytes): 42907.8608*

| Scenario | Time | IO Read KBytes/sec | Current Disk Queue Length | Page Faults Faults/sec | Disk Queue Length Count | Total .Net Memory Mb | Concurrent Threads  | Current lock Queue Length | Available MBytes Mb | Disk Read Kb / sec | Disk Read Time  | CPU Load % | Threads Count | Was failed |
| -------- | -------- | -------- | -------- | -------- |
| ScenarioAsyncWithMaxParallelCount4 | 00:00:00.9570000 | 146142.80 | 0.00 | 65333.20 | 0.00 | 72.40 | 16.20 | 1.00 | 5676.40 | 29724550.40 | 0.00 | 11762475.40 | 19.20 | False |
| ScenarioAsyncWithMaxParallelCount8 | 00:00:00.8400000 | 154918.00 | 0.00 | 65106.20 | 0.00 | 119.60 | 27.60 | 1.00 | 5696.20 | 29724870.00 | 0.00 | 13728088.00 | 30.80 | False |
| ScenarioAsyncWithMaxParallelCount16 | 00:00:00.9360000 | 133720.00 | 0.00 | 54467.80 | 0.00 | 120.20 | 28.00 | 1.00 | 5588.00 | 29726177.60 | 0.00 | 22526544.40 | 31.00 | False |
| ScenarioAsyncWithMaxParallelCount32 | 00:00:00.2090000 | 210847.40 | 0.00 | 85108.50 | 0.00 | 96.30 | 33.70 | 1.00 | 5677.70 | 29728649.10 | 0.00 | 54132347.00 | 36.70 | False |
| ScenarioAsyncWithMaxParallelCount64 | 00:00:00.5970000 | 239390.90 | 0.00 | 112040.90 | 0.00 | 78.70 | 35.30 | 1.00 | 5692.10 | 29732225.90 | 0.00 | 77938099.60 | 38.60 | False |
| ScenarioAsyncWithMaxParallelCount128 | 00:00:00.6600000 | 228823.60 | 0.00 | 131848.20 | 0.00 | 86.90 | 37.30 | 1.00 | 5676.90 | 29736267.10 | 0.00 | 70949254.80 | 40.50 | False |
| ScenarioAsyncWithMaxParallelCount256 | 00:00:00.7790000 | 211313.40 | 0.00 | 149404.20 | 0.00 | 98.30 | 37.20 | 1.00 | 5670.40 | 29740235.40 | 0.00 | 69982048.60 | 40.50 | False |
| ScenarioSyncAsParallel | 00:00:00.8840000 | 127209.40 | 0.00 | 51897.20 | 0.00 | 8.40 | 27.00 | 0.00 | 5717.40 | 29743043.80 | 0.00 | 18626519.40 | 30.00 | False |
| ScenarioReadAllAsParallel | 00:00:00.3280000 | 294095.40 | 0.00 | 59385.20 | 0.00 | 58.40 | 27.00 | 0.00 | 5582.60 | 29745002.80 | 0.00 | 21964940.80 | 30.00 | False |
| ScenarioAsync | 00:00:00.8790000 | 92711.10 | 0.00 | 196751.80 | 0.00 | 652.00 | 31.30 | 0.50 | 5160.80 | 29754140.00 | 0.00 | 74521677.70 | 36.70 | False |
| ScenarioAsync2 | 00:00:00.4340000 | 193810.40 | 0.00 | 75056.80 | 0.00 | 182.20 | 28.00 | 0.00 | 5514.60 | 29786128.40 | 0.00 | 18033715.60 | 31.00 | False |
| ScenarioNewThread | 00:00:00.6220000 | 200139.30 | 0.00 | 97247.80 | 0.00 | 19.00 | 106.10 | 0.00 | 5706.10 | 29787087.30 | 0.00 | 157873012.00 | 78.00 | False |

*Min size (bytes): 10007 bytes, max size (bytes): 62444171, average size (bytes): 205102.2773*

| Scenario | Time | IO Read KBytes/sec | Current Disk Queue Length | Page Faults Faults/sec | Disk Queue Length Count | Total .Net Memory Mb | Concurrent Threads  | Current lock Queue Length | Available MBytes Mb | Disk Read Kb / sec | Disk Read Time  | CPU Load % | Threads Count | Was failed |
| -------- | -------- | -------- | -------- | -------- |
| ScenarioAsyncWithMaxParallelCount4 | 00:00:00.6460000 | 933722.47 | 0.00 | 314479.67 | 0.00 | 91.27 | 17.67 | 1.00 | 5520.13 | 31195428.53 | 0.00 | 55723557.20 | 20.80 | False |
| ScenarioAsyncWithMaxParallelCount8 | 00:00:00.4400000 | 1014042.40 | 0.00 | 336146.40 | 0.00 | 163.07 | 28.27 | 1.00 | 5539.00 | 31196335.93 | 0.00 | 73299669.87 | 31.73 | False |
| ScenarioAsyncWithMaxParallelCount16 | 00:00:00.6990000 | 894781.87 | 0.00 | 286889.87 | 0.00 | 295.20 | 35.00 | 1.00 | 5326.33 | 31196938.80 | 0.00 | 104728671.33 | 38.60 | False |
| ScenarioAsyncWithMaxParallelCount32 | 00:00:00.8170000 | 853412.73 | 0.00 | 274329.27 | 0.00 | 329.53 | 40.27 | 1.00 | 5339.53 | 31197021.73 | 0.00 | 124031195.07 | 43.67 | False |
| ScenarioAsyncWithMaxParallelCount64 | 00:00:00.1170000 | 751968.67 | 0.00 | 248434.20 | 0.00 | 257.95 | 39.63 | 1.00 | 5385.97 | 31197232.13 | 0.00 | 126953613.80 | 41.13 | False |
| ScenarioAsyncWithMaxParallelCount128 | 00:00:00.2750000 | 878507.05 | 0.00 | 320178.28 | 0.00 | 258.67 | 39.83 | 1.00 | 5410.37 | 31197414.30 | 0.00 | 152535177.78 | 43.38 | False |
| ScenarioAsyncWithMaxParallelCount256 | 00:00:00.3740000 | 959797.15 | 0.00 | 387836.25 | 0.00 | 284.22 | 40.97 | 1.00 | 5382.10 | 31197702.95 | 0.00 | 156094600.60 | 44.13 | False |
| ScenarioSyncAsParallel | 00:00:00.5060000 | 1034339.20 | 0.00 | 422385.07 | 0.00 | 139.93 | 27.00 | 0.33 | 5517.27 | 31198580.40 | 0.00 | 83491735.20 | 30.00 | False |
| ScenarioReadAllAsParallel | 00:00:00.9050000 | 518228.80 | 0.00 | 98020.80 | 0.00 | 242.00 | 27.00 | 0.00 | 5341.80 | 31198730.60 | 0.00 | 23306549.40 | 30.00 | False |
| ScenarioAsync | 00:00:00.3270000 | 685087.50 | 0.00 | 449445.69 | 0.00 | 788.40 | 38.97 | 0.77 | 5220.14 | 31199763.45 | 0.00 | 165039697.94 | 40.35 | False |
| ScenarioAsync2 | 00:00:00.5060000 | 1005440.20 | 0.00 | 334943.60 | 0.00 | 396.60 | 40.13 | 0.00 | 5469.40 | 31200809.80 | 0.00 | 83013332.13 | 43.67 | False |
| ScenarioNewThread | 00:00:00.4940000 | 1051794.10 | 0.00 | 360434.45 | 0.00 | 159.10 | 122.05 | 0.25 | 5708.90 | 31201001.00 | 0.00 | 360409110.30 | 73.80 | False |

*Min size (bytes): 100025 bytes, max size (bytes): 425938432, average size (bytes): 1583188.7766*

| Scenario | Time | IO Read KBytes/sec | Current Disk Queue Length | Page Faults Faults/sec | Disk Queue Length Count | Total .Net Memory Mb | Concurrent Threads  | Current lock Queue Length | Available MBytes Mb | Disk Read Kb / sec | Disk Read Time  | CPU Load % | Threads Count | Was failed |
| -------- | -------- | -------- | -------- | -------- |
| ScenarioAsyncWithMaxParallelCount4 | 00:00:00 |  | True |
| ScenarioAsyncWithMaxParallelCount8 | 00:00:00 |  | True |
| ScenarioAsyncWithMaxParallelCount16 | 00:00:00.4510000 | 8029271.23 | 34.19 | 2956452.44 | 0.00 | 781.15 | 38.34 | 1.00 | 5958.68 | 239191771.93 | 0.00 | 478121377.06 | 43.70 | False |
| ScenarioAsyncWithMaxParallelCount32 | 00:00:00 |  | True |
| ScenarioAsyncWithMaxParallelCount64 | 00:00:00 |  | True |
| ScenarioAsyncWithMaxParallelCount128 | 00:00:00 |  | True |
| ScenarioAsyncWithMaxParallelCount256 | 00:00:00 |  | True |
| ScenarioSyncAsParallel | 00:00:00 |  | True |
| ScenarioReadAllAsParallel | 00:00:00 |  | True |
| ScenarioAsync | 00:00:00 |  | True |
| ScenarioAsync2 | 00:00:00 |  | True |
| ScenarioNewThread | 00:00:00 |  | True |

*Min size (bytes): 1000444 bytes, max size (bytes): 425938432, average size (bytes): 4734577.12538997*

| Scenario | Time | IO Read KBytes/sec | Current Disk Queue Length | Page Faults Faults/sec | Disk Queue Length Count | Total .Net Memory Mb | Concurrent Threads  | Current lock Queue Length | Available MBytes Mb | Disk Read Kb / sec | Disk Read Time  | CPU Load % | Threads Count | Was failed |
| -------- | -------- | -------- | -------- | -------- |
| ScenarioAsyncWithMaxParallelCount4 | 00:00:00 |  | True |
| ScenarioAsyncWithMaxParallelCount8 | 00:00:00 |  | True |
| ScenarioAsyncWithMaxParallelCount16 | 00:00:00.5490000 | 19233085.82 | 36.76 | 8075379.44 | 0.00 | 690.80 | 36.66 | 1.00 | 11257.30 | 1580546144.16 | 0.00 | 1069448571.21 | 41.05 | False |
| ScenarioAsyncWithMaxParallelCount32 | 00:00:00 |  | True |
| ScenarioAsyncWithMaxParallelCount64 | 00:00:00 |  | True |
| ScenarioAsyncWithMaxParallelCount128 | 00:00:00 |  | True |
| ScenarioAsyncWithMaxParallelCount256 | 00:00:00.3340000 | 19407022.38 | 68.56 | 7500852.37 | 0.00 | 1777.42 | 47.95 | 1.00 | 9947.98 | 2479331952.26 | 0.00 | 697129809.19 | 52.29 | False |
| ScenarioSyncAsParallel | 00:00:00 |  | True |
| ScenarioReadAllAsParallel | 00:00:00.2470000 | 20028004.43 | 120.80 | 3180493.36 | 0.00 | 319.69 | 25.29 | 0.37 | 10891.96 | 2734291138.72 | 0.00 | 337623756.69 | 29.46 | False |
| ScenarioAsync | 00:00:00 |  | True |
| ScenarioAsync2 | 00:00:00 |  | True |
| ScenarioNewThread | 00:00:00 |  | True |


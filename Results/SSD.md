# Results of SSD disk check

**Check 10000 files to read**

*Min size (bytes): 6 bytes, max size (bytes): 71185624, average size (bytes): 200447,6022*

| Scenario | Time | CPU usage (%) | Memory usage (Mb) | Was failed |
| -------- | -------- | -------- | -------- | -------- |
| ScenarioAsyncWithMaxParallelCount4 | 00:00:03.1800000 | 221,8768 | **6,27923** | False |
| ScenarioAsyncWithMaxParallelCount8 | 00:00:03.1400000 | 227,5311 | 9,340498 | False |
| ScenarioAsyncWithMaxParallelCount16 | 00:00:03.1080000 | 222,6875 | 12,60546 | False |
| ScenarioAsyncWithMaxParallelCount32 | 00:00:03.1780000 | 218,8443 | 12,3188 | False |
| ScenarioAsyncWithMaxParallelCount64 | 00:00:03.0890000 | 215,6534 | 14,62445 | False |
| ScenarioAsyncWithMaxParallelCount128 | 00:00:03.2690000 | 215,3714 | 12,46841 | False |
| ScenarioAsyncWithMaxParallelCount256 | 00:00:03.4450000 | 216,1703 | 12,23651 | False |
| ScenarioSyncAsParallel | 00:00:03.1240000 | 176,8006 | 7,585711 | False |
| ScenarioReadAllAsParallel | **00:00:01.3900000** | **136,8101** | 10,54994 | False |
| ScenarioAsync | 00:00:03.6530000 | 208,2858 | 53,61056 | False |
| ScenarioAsync2 | 00:00:03.5750000 | 212,6839 | 43,59129 | False |
| ScenarioNewThread | 00:00:04.0080000 | 256,4063 | 8,963113 | False |

*Min size (bytes): 1001 bytes, max size (bytes): 71185624, average size (bytes): 214027,6364*

| Scenario | Time | CPU usage (%) | Memory usage (Mb) | Was failed |
| -------- | -------- | -------- | -------- | -------- |
| ScenarioAsyncWithMaxParallelCount4 | 00:00:03.2080000 | 215,7142 | **6,333112** | False |
| ScenarioAsyncWithMaxParallelCount8 | 00:00:03.2490000 | 221,1309 | 8,159969 | False |
| ScenarioAsyncWithMaxParallelCount16 | 00:00:03.1820000 | 224,214 | 11,27139 | False |
| ScenarioAsyncWithMaxParallelCount32 | 00:00:03.2350000 | 217,8882 | 12,09145 | False |
| ScenarioAsyncWithMaxParallelCount64 | 00:00:03.3040000 | 217,4626 | 10,74947 | False |
| ScenarioAsyncWithMaxParallelCount128 | 00:00:03.3810000 | 219,6199 | 15,01676 | False |
| ScenarioAsyncWithMaxParallelCount256 | 00:00:03.4660000 | 216,2823 | 18,28049 | False |
| ScenarioSyncAsParallel | 00:00:03.5270000 | 159,4497 | 4,917924 | False |
| ScenarioReadAllAsParallel | **00:00:01.4780000** | **128,9311** | 7,296205 | False |
| ScenarioAsync | 00:00:03.8350000 | 203,5782 | 65,71413 | False |
| ScenarioAsync2 | 00:00:03.7480000 | 222,5106 | 50,48918 | False |
| ScenarioNewThread | 00:00:04.0880000 | 261,6721 | 9,38526 | False |

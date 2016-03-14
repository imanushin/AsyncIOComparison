# Results of HDD disk check

**Check 10000 files to read**

*Min size (bytes): 4 bytes, max size (bytes): 52199996, average size (bytes): 207685,1133*

| Scenario | Time | CPU usage (%) | Memory usage (Mb) | Was failed |
| -------- | -------- | -------- | -------- | -------- |
| ScenarioAsyncWithMaxParallelCount4 | 00:00:19.7790000 | 69,31738 | 11,22953 | False |
| ScenarioAsyncWithMaxParallelCount8 | 00:00:31.6590000 | 32,80133 | 13,00693 | False |
| ScenarioAsyncWithMaxParallelCount16 | 00:00:33.2620000 | 36,51301 | 12,38325 | False |
| ScenarioAsyncWithMaxParallelCount32 | 00:00:03.9460000 | 158,3389 | 11,53394 | False |
| ScenarioAsyncWithMaxParallelCount64 | 00:00:03.4380000 | 167,223 | 14,6197 | False |
| ScenarioAsyncWithMaxParallelCount128 | 00:00:03.4980000 | 161,5632 | 14,46945 | False |
| ScenarioAsyncWithMaxParallelCount256 | 00:00:08.4830000 | 124,6479 | 15,17151 | False |
| ScenarioSyncAsParallel | 00:00:03.0290000 | 167,0058 | 6,297935 | False |
| ScenarioReadAllAsParallel | 00:00:01.3990000 | 143,0702 | 6,174474 | False |
| ScenarioAsync | 00:00:27.2250000 | 35,49822 | 43,84406 | False |
| ScenarioAsync2 | 00:01:07.4310000 | 42,61198 | 82,66186 | False |
| ScenarioNewThread | 00:00:06.7910000 | 235,5897 | 17,69374 | False |

*Min size (bytes): 1001 bytes, max size (bytes): 52199996, average size (bytes): 208758,0093*

| Scenario | Time | CPU usage (%) | Memory usage (Mb) | Was failed |
| -------- | -------- | -------- | -------- | -------- |
| ScenarioAsyncWithMaxParallelCount4 | 00:00:02.9980000 | 183,3214 | 10,3315 | False |
| ScenarioAsyncWithMaxParallelCount8 | 00:00:03.9430000 | 159,8664 | 18,0295 | False |
| ScenarioAsyncWithMaxParallelCount16 | 00:00:02.9170000 | 183,2876 | 11,32259 | False |
| ScenarioAsyncWithMaxParallelCount32 | 00:00:03.0250000 | 180,2098 | 14,01685 | False |
| ScenarioAsyncWithMaxParallelCount64 | 00:00:04.4550000 | 163,4882 | 17,76796 | False |
| ScenarioAsyncWithMaxParallelCount128 | 00:00:09.7450000 | 96,84496 | 14,21254 | False |
| ScenarioAsyncWithMaxParallelCount256 | 00:00:03.3710000 | 185,5446 | 17,12878 | False |
| ScenarioSyncAsParallel | 00:00:03.0190000 | 168,789 | 4,761682 | False |
| ScenarioReadAllAsParallel | 00:00:01.3570000 | 147,2238 | 12,57452 | False |
| ScenarioAsync | 00:00:29.8600000 | 32,27755 | 45,87634 | False |
| ScenarioAsync2 | 00:00:31.3750000 | 115,9477 | 60,77231 | False |
| ScenarioNewThread | 00:00:07.7810000 | 236,956 | 19,10081 | False |

*Min size (bytes): 100072 bytes, max size (bytes): 143659408, average size (bytes): 1006091,6523*

| Scenario | Time | CPU usage (%) | Memory usage (Mb) | Was failed |
| -------- | -------- | -------- | -------- | -------- |
| ScenarioAsyncWithMaxParallelCount4 | 00:05:38.7950000 | 15,40258 | 10,88277 | False |
| ScenarioAsyncWithMaxParallelCount8 | 00:05:28.5910000 | 18,8377 | 15,59943 | False |
| ScenarioAsyncWithMaxParallelCount16 | 00:07:42.9600000 | 15,07634 | 27,67022 | False |
| ScenarioAsyncWithMaxParallelCount32 | 00:08:07.8280000 | 13,86182 | 45,39138 | False |
| ScenarioAsyncWithMaxParallelCount64 | 00:08:47.8960000 | 11,17171 | 49,16019 | False |
| ScenarioAsyncWithMaxParallelCount128 | 00:00:00 | 0 | 0 | True |
| ScenarioAsyncWithMaxParallelCount256 | 00:00:00 | 0 | 0 | True |
| ScenarioSyncAsParallel | 00:00:00 | 0 | 0 | True |

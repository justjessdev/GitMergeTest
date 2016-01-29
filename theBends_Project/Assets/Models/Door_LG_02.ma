//Maya ASCII 2015 scene
//Name: Door_LG_02.ma
//Last modified: Fri, Nov 06, 2015 01:23:29 PM
//Codeset: 1252
requires maya "2015";
currentUnit -l meter -a degree -t film;
fileInfo "application" "maya";
fileInfo "product" "Maya 2015";
fileInfo "version" "2015";
fileInfo "cutIdentifier" "201503261530-955654";
fileInfo "osv" "Microsoft Windows 7 Enterprise Edition, 64-bit Windows 7 Service Pack 1 (Build 7601)\n";
fileInfo "license" "education";
createNode transform -n "Door_LG_02";
createNode mesh -n "Door_LG_02Shape" -p "Door_LG_02";
	setAttr -k off ".v";
	setAttr ".iog[0].og[0].gcl" -type "componentList" 1 "f[0:97]";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.5 0.375 ;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr -s 176 ".uvst[0].uvsp[0:175]" -type "float2" 0.375 0.5 0.625 0.25
		 0.625 0.5 0.375 0.25 0.625 0.25 0.625 0.5 0.375 0.5 0.375 0.25 0.625 0.25 0.625 0.5
		 0.375 0.5 0.375 0.25 0.625 0.25 0.625 0.5 0.375 0.5 0.375 0.25 0.375 0.5 0.375 0.25
		 0.625 0.25 0.625 0.5 0.375 0.5 0.375 0.25 0.625 0.25 0.625 0.5 0.375 0.5 0.375 0.25
		 0.375 0.25 0.625 0.25 0.625 0.5 0.375 0.25 0.625 0.25 0.625 0.5 0.375 0.5 0.375 0.5
		 0.375 0.25 0.375 0.5 0.625 0.25 0.375 0.25 0.625 0.25 0.375 0.5 0.375 0.25 0.375
		 0.5 0.375 0.25 0.625 0.5 0.375 0.25 0.625 0.5 0.375 0.5 0.47367072 0.25 0.47367072
		 0.5 0.5 0.38427323 0.375 0.5 0.47367072 0.38273901 0.375 0.5 0.625 0.25 0.625 0.5
		 0.375 0.25 0.625 0.25 0.625 0.5 0.375 0.5 0.375 0.25 0.625 0.25 0.625 0.5 0.375 0.5
		 0.375 0.25 0.625 0.25 0.625 0.5 0.375 0.5 0.47367072 0.38273901 0.375 0.25 0.375
		 0.25 0.625 0.25 0.625 0.5 0.375 0.5 0.375 0.25 0.625 0.25 0.5 0.38427323 0.625 0.5
		 0.375 0.5 0.47367072 0.25 0.375 0.25 0.375 0.25 0.625 0.25 0.625 0.5 0.375 0.25 0.625
		 0.25 0.625 0.5 0.375 0.5 0.375 0.25 0.625 0.5 0.375 0.5 0.375 0.25 0.625 0.25 0.375
		 0.5 0.375 0.25 0.625 0.25 0.375 0.5 0.375 0.25 0.375 0.5 0.375 0.25 0.625 0.5 0.375
		 0.5 0.47367072 0.5 0.6068235 0.25 0.6068235 0.5 0.6068235 0.5 0.6068235 0.5 0.6068235
		 0.5 0.6068235 0.5 0.6068235 0.5 0.6068235 0.25 0.60444951 0.5 0.60444951 0.25 0.60444951
		 0.5 0.60444951 0.25 0.60444951 0.25 0.60444951 0.25 0.60074311 0.25 0.60074311 0.25
		 0.60074311 0.25 0.60074311 0.5 0.60074311 0.25 0.60074311 0.5 0.6068235 0.5 0.60074311
		 0.25 0.5 0.38427323 0.625 0.5 0.47367072 0.25 0.375 0.25 0.47367072 0.38273901 0.47367072
		 0.5 0.625 0.25 0.625 0.5 0.6068235 0.5 0.6068235 0.25 0.60074311 0.5 0.60074311 0.25
		 0.625 0.25 0.625 0.5 0.60074311 0.5 0.60074311 0.25 0.5 0.38427323 0.47367072 0.25
		 0.47367072 0.5 0.47367072 0.38273901 0.375 0.25 0.6068235 0.5 0.60074311 0.25 0.5
		 0.38427323 0.625 0.5 0.47367072 0.25 0.375 0.25 0.47367072 0.38273901 0.47367072
		 0.5 0.625 0.25 0.625 0.5 0.6068235 0.5 0.6068235 0.25 0.6068235 0.25 0.6068235 0.5
		 0.625 0.5 0.625 0.25 0.60444951 0.25 0.60444951 0.5 0.60444951 0.25 0.60444951 0.5
		 0.625 0.5 0.625 0.25 0.60444951 0.25 0.6068235 0.5 0.625 0.5 0.60074311 0.5 0.60074311
		 0.25 0.625 0.25 0.625 0.5 0.60074311 0.5 0.60074311 0.25;
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
	setAttr -s 30 ".pt";
	setAttr ".pt[12]" -type "float3" 0 0 2.3841857e-009 ;
	setAttr ".pt[13]" -type "float3" 0 0 2.3841857e-009 ;
	setAttr ".pt[16]" -type "float3" 0 0 2.3841857e-009 ;
	setAttr ".pt[19]" -type "float3" 0 -9.5367433e-008 2.3841857e-009 ;
	setAttr ".pt[21]" -type "float3" 0 0 2.3841857e-009 ;
	setAttr ".pt[22]" -type "float3" 0 0 2.3841857e-009 ;
	setAttr ".pt[23]" -type "float3" 0 0 2.3841857e-009 ;
	setAttr ".pt[24]" -type "float3" 0 0 2.3841857e-009 ;
	setAttr ".pt[53]" -type "float3" 0 0 2.3841857e-009 ;
	setAttr ".pt[54]" -type "float3" 0 0 2.3841857e-009 ;
	setAttr ".pt[55]" -type "float3" -1.9073486e-008 0 2.3841857e-009 ;
	setAttr ".pt[56]" -type "float3" 0 0 2.3841857e-009 ;
	setAttr ".pt[57]" -type "float3" 0 0 2.3841857e-009 ;
	setAttr ".pt[63]" -type "float3" 0 0 -1.2665987e-009 ;
	setAttr ".pt[64]" -type "float3" 0 0 -1.2665987e-009 ;
	setAttr ".pt[65]" -type "float3" 0 0 -1.2665987e-009 ;
	setAttr ".pt[66]" -type "float3" 0 0 -1.2665987e-009 ;
	setAttr ".pt[67]" -type "float3" 0 0 -1.2665987e-009 ;
	setAttr ".pt[68]" -type "float3" 0 0 -1.2665987e-009 ;
	setAttr ".pt[69]" -type "float3" 0 0 -1.2665987e-009 ;
	setAttr ".pt[70]" -type "float3" 0 0 -1.2665987e-009 ;
	setAttr ".pt[71]" -type "float3" 0 0 -1.2665987e-009 ;
	setAttr ".pt[72]" -type "float3" 0 0 -1.2665987e-009 ;
	setAttr ".pt[73]" -type "float3" 0 0 -1.2665987e-009 ;
	setAttr ".pt[74]" -type "float3" 0 0 -1.2665987e-009 ;
	setAttr ".pt[75]" -type "float3" 0 0 -1.2665987e-009 ;
	setAttr -s 100 ".vt[0:99]"  -8.5265126e-016 0 -1 -8.5265126e-016 1 -1
		 1 0 -1 2 0 -1 -8.5265126e-016 2 -1 -8.5265126e-016 3 -1 -8.5265126e-016 4 -1 1 4 -1
		 2 4 -1 0.1042423 1.052121162 -0.97393942 1.052121162 0.1042423 -0.97393942 1.052121162 1.052121162 -0.97393942
		 2 0.1042423 -0.97393942 2 1.052121162 -0.97393942 1.052121162 2 -0.97393942 0.1042423 2 -0.97393942
		 1.052121282 2.94787884 -0.97393942 0.1042423 2.94787884 -0.97393942 2 2.94787884 -0.97393942
		 1.052121162 3.89575768 -0.97393942 2 3.89575768 -0.97393942 2.37411141 2 -0.97393942
		 2 2.50909901 -0.97393942 2.37411141 1.55540311 -0.97393942 -8.5265126e-016 0 2.3841857e-009
		 -8.5265126e-016 1 -2.220446e-016 1 0 0 2 0 0 -8.5265126e-016 2 -4.4408921e-016 -8.5265126e-016 3 -6.6613381e-016
		 -8.5265126e-016 4 -8.8817842e-016 1 4 -8.8817842e-016 2 4 -8.8817842e-016 0.1042423 1.052121162 -0.026060583
		 1.052121162 0.1042423 -0.026060583 1.052121162 1.052121162 -0.026060583 2 0.1042423 -0.026060583
		 2 1.052121162 -0.026060583 1.052121162 2 -0.026060583 0.1042423 2 -0.026060583 1.052121282 2.94787884 -0.026060583
		 0.1042423 2.94787884 -0.026060583 2 2.94787884 -0.026060583 1.052121162 3.89575768 -0.026060583
		 2 3.89575768 -0.026060583 2.37411141 2 -0.026060583 2 2.50909901 -0.026060583 2.37411141 1.55540311 -0.026060583
		 1.93108368 2.94787884 -0.026060583 1.93108368 3.89575768 -0.026060583 1.92729414 4 -8.8817842e-016
		 1.92729414 4 -1 1.93108368 3.89575768 -0.97393942 1.93108368 2.94787884 -0.97393942
		 1.92208278 2 -0.97393942 1.92208278 1.052121162 -0.97393942 1.92208278 0.1042423 -0.97393942
		 1.91779828 0 -1 1.90297246 0 0 1.90802968 0.1042423 -0.026060583 1.90802968 1.052121162 -0.026060583
		 1.90802968 2 -0.026060583 1.93108368 2.94787884 -0.026060583 1.90802968 2 -0.026060587
		 2 2.50909901 -0.026060587 2 2.94787884 -0.026060587 2.37411141 2 -0.026060587 2.37411141 1.55540311 -0.026060587
		 2 1.052121162 -0.026060587 2 3.89575768 -0.026060587 1.93108368 3.89575768 -0.026060587
		 1.90802968 0.1042423 -0.026060587 1.90802968 1.052121162 -0.026060587 2 0.1042423 -0.026060587
		 2 2 -1.0029394627 2.37411141 2 -1.0029394627 2 2.50909901 -1.0029394627 2 1.052121162 -1.0029394627
		 2.37411141 1.55540311 -1.0029394627 1.93108368 3.89575768 -1.0029394627 1.93108368 2.94787884 -1.0029394627
		 2 3.89575768 -1.0029394627 2 2.94787884 -1.0029394627 1.92208278 2 -1.0029394627
		 1.92208278 1.052121162 -1.0029394627 1.92208278 0.1042423 -1.0029394627 2 0.1042423 -1.0029394627
		 1.93108368 2.94787884 0.0029394149 1.90802968 2 0.0029394149 2 2 0.0029394149 2 2.50909901 0.0029394149
		 2 2.94787884 0.0029394149 2.37411141 2 0.0029394149 2.37411141 1.55540311 0.0029394149
		 2 1.052121162 0.0029394149 2 3.89575768 0.0029394149 1.93108368 3.89575768 0.0029394149
		 1.90802968 0.1042423 0.0029394149 1.90802968 1.052121162 0.0029394149 2 0.1042423 0.0029394149;
	setAttr -s 198 ".ed";
	setAttr ".ed[0:165]"  0 1 0 0 2 0 2 57 0 1 4 0 4 5 0 5 6 0 6 7 0 7 51 0 7 5 0
		 1 2 0 1 9 0 2 10 0 9 10 0 10 11 0 9 11 0 3 12 0 10 56 0 12 13 1 11 55 0 11 14 0 4 15 0
		 15 14 0 9 15 0 14 54 0 14 16 0 5 17 0 17 16 0 15 17 0 16 53 0 7 19 0 16 19 0 19 17 0
		 8 20 0 18 20 1 19 52 0 22 18 1 23 21 1 22 21 1 23 13 1 24 25 0 24 26 0 26 58 0 25 28 0
		 28 29 0 29 30 0 30 31 0 31 50 0 31 29 0 25 26 0 25 33 0 26 34 0 33 34 0 34 35 0 33 35 0
		 27 36 0 34 59 0 36 37 0 35 60 0 35 38 0 28 39 0 39 38 0 33 39 0 38 61 0 38 40 0 29 41 0
		 41 40 0 39 41 0 40 48 0 31 43 0 40 43 0 43 41 0 32 44 0 42 44 0 43 49 0 46 42 0 47 45 0
		 46 45 0 47 37 0 0 24 0 6 30 0 5 29 1 4 28 1 1 25 1 8 32 0 7 31 1 20 44 0 22 46 0
		 18 42 1 3 27 0 12 36 1 13 37 0 23 47 0 21 45 0 49 44 0 48 49 0 50 32 0 49 50 1 51 8 0
		 50 51 1 52 20 0 51 52 1 52 53 0 54 55 0 56 12 0 55 56 0 57 3 0 56 57 1 53 54 0 58 27 0
		 59 36 0 58 59 1 59 60 0 60 61 0 48 61 0 48 62 0 61 63 0 62 63 0 46 64 0 42 65 0 64 65 0
		 45 66 0 64 66 0 47 67 0 37 68 0 67 68 0 67 66 0 44 69 0 65 69 0 49 70 0 70 69 0 62 70 0
		 59 71 0 60 72 0 71 72 0 36 73 0 71 73 0 73 68 0 72 63 0 21 75 0 74 75 0 22 76 0 74 76 0
		 76 75 0 13 77 0 77 74 0 23 78 0 78 75 0 78 77 0 52 79 0 53 80 1 79 80 0 20 81 0 79 81 0
		 18 82 1 82 81 0 80 82 0 54 83 1 55 84 1 83 84 0 83 74 0 84 77 0 56 85 0 84 85 0 12 86 0
		 86 77 0 85 86 0;
	setAttr ".ed[166:197]" 80 83 0 76 82 0 62 87 1 63 88 1 87 88 0 88 89 0 64 90 0
		 89 90 0 65 91 1 90 91 0 87 91 0 66 92 0 90 92 0 89 92 0 67 93 0 68 94 0 93 94 0 93 92 0
		 94 89 0 69 95 0 91 95 0 70 96 0 96 95 0 87 96 0 71 97 0 72 98 1 97 98 0 73 99 0 97 99 0
		 99 94 0 98 94 0 98 88 0;
	setAttr -s 98 -ch 384 ".fc[0:97]" -type "polyFaces" 
		f 3 14 -14 -13
		mu 0 3 35 2 36
		f 4 13 18 104 -17
		mu 0 4 37 6 112 114
		f 4 22 21 -20 -15
		mu 0 4 40 39 9 8
		f 4 19 23 102 -19
		mu 0 4 11 14 110 113
		f 4 27 26 -25 -22
		mu 0 4 42 41 19 18
		f 4 24 28 107 -24
		mu 0 4 21 24 108 111
		f 3 -32 -31 -27
		mu 0 3 44 43 27
		f 4 30 34 101 -29
		mu 0 4 29 46 107 109
		f 3 5 6 8
		mu 0 3 26 33 28
		f 3 0 9 -2
		mu 0 3 34 0 1
		f 4 10 12 -12 -10
		mu 0 4 0 35 36 1
		f 4 11 16 106 -3
		mu 0 4 3 37 114 115
		f 4 20 -23 -11 3
		mu 0 4 10 39 40 7
		f 4 25 -28 -21 4
		mu 0 4 20 41 42 17
		f 4 29 31 -26 -9
		mu 0 4 28 43 44 26
		f 4 100 -35 -30 7
		mu 0 4 106 107 46 32
		f 3 -140 141 142
		mu 0 3 141 25 140
		f 4 144 139 -147 147
		mu 0 4 144 16 142 143
		f 3 51 52 -54
		mu 0 3 52 53 54
		f 4 55 111 -58 -53
		mu 0 4 55 117 119 58
		f 4 53 58 -61 -62
		mu 0 4 59 60 61 62
		f 4 57 112 -63 -59
		mu 0 4 63 118 121 66
		f 4 60 63 -66 -67
		mu 0 4 69 70 71 72
		f 5 170 171 173 175 -177
		mu 0 5 145 146 74 147 148
		f 3 65 69 70
		mu 0 3 80 81 82
		f 4 67 94 -74 -70
		mu 0 4 83 102 104 86
		f 3 -48 -46 -45
		mu 0 3 87 88 89
		f 3 40 -49 -40
		mu 0 3 90 91 92
		f 4 48 50 -52 -50
		mu 0 4 92 91 53 52
		f 4 41 110 -56 -51
		mu 0 4 93 116 117 55
		f 4 -43 49 61 -60
		mu 0 4 95 96 59 62
		f 4 -44 59 66 -65
		mu 0 4 97 98 69 72
		f 4 47 64 -71 -69
		mu 0 4 88 87 80 82
		f 4 -47 68 73 96
		mu 0 4 105 100 86 104
		f 3 -179 -174 179
		mu 0 3 149 147 79
		f 4 -183 183 -180 -185
		mu 0 4 150 151 152 50
		f 4 -6 80 44 -80
		mu 0 4 33 26 87 89
		f 4 -5 81 43 -81
		mu 0 4 20 17 98 97
		f 4 -4 82 42 -82
		mu 0 4 10 7 96 95
		f 4 -1 78 39 -83
		mu 0 4 0 34 90 92
		f 4 -8 84 46 98
		mu 0 4 106 32 100 105
		f 4 -7 79 45 -85
		mu 0 4 28 33 89 88
		f 4 35 87 -75 -87
		mu 0 4 49 23 76 75
		f 4 33 85 -73 -88
		mu 0 4 30 45 85 84
		f 4 15 89 -55 -89
		mu 0 4 4 38 56 94
		f 4 17 90 -57 -90
		mu 0 4 38 5 57 56
		f 4 -39 91 77 -91
		mu 0 4 15 51 67 68
		f 4 36 92 -76 -92
		mu 0 4 51 48 101 67
		f 4 -38 86 76 -93
		mu 0 4 47 49 75 78
		f 4 176 186 -189 -190
		mu 0 4 156 153 154 155
		f 4 -96 -97 93 -72
		mu 0 4 99 105 104 85
		f 4 -98 -99 95 -84
		mu 0 4 31 106 105 99
		f 4 32 -100 -101 97
		mu 0 4 31 45 107 106
		f 4 -151 152 -155 -156
		mu 0 4 157 158 159 160
		f 4 -159 159 -145 -161
		mu 0 4 161 162 13 12
		f 4 -163 160 -165 -166
		mu 0 4 163 164 165 166
		f 4 -107 103 -16 -106
		mu 0 4 115 114 38 4
		f 5 -167 155 -168 -142 -160
		mu 0 5 167 168 169 140 22
		f 4 108 54 -110 -111
		mu 0 4 116 94 56 117
		f 4 -193 194 195 -197
		mu 0 4 170 171 172 173
		f 4 -198 196 184 -172
		mu 0 4 174 175 64 65
		f 4 62 -114 -68 -64
		mu 0 4 73 120 103 77
		f 4 113 115 -117 -115
		mu 0 4 103 120 123 122
		f 4 74 118 -120 -118
		mu 0 4 75 76 125 124
		f 4 -77 117 121 -121
		mu 0 4 78 75 124 126
		f 4 -78 122 124 -124
		mu 0 4 68 67 128 127
		f 4 75 120 -126 -123
		mu 0 4 67 101 129 128
		f 4 72 126 -128 -119
		mu 0 4 84 85 131 130
		f 4 -94 128 129 -127
		mu 0 4 85 104 132 131
		f 4 -95 114 130 -129
		mu 0 4 104 102 133 132
		f 4 -112 131 133 -133
		mu 0 4 119 117 135 134
		f 4 109 134 -136 -132
		mu 0 4 117 56 136 135
		f 4 56 123 -137 -135
		mu 0 4 56 57 137 136
		f 4 -113 132 137 -116
		mu 0 4 121 118 139 138
		f 4 37 138 -143 -141
		mu 0 4 49 47 141 140
		f 4 -37 145 146 -139
		mu 0 4 48 51 143 142
		f 4 38 143 -148 -146
		mu 0 4 51 15 144 143
		f 4 -102 148 150 -150
		mu 0 4 122 123 146 145
		f 4 99 151 -153 -149
		mu 0 4 124 125 148 147
		f 4 -34 153 154 -152
		mu 0 4 126 124 147 149
		f 4 -103 156 158 -158
		mu 0 4 127 128 151 150
		f 4 -105 157 162 -162
		mu 0 4 128 129 152 151
		f 4 -18 163 164 -144
		mu 0 4 130 131 154 153
		f 4 -104 161 165 -164
		mu 0 4 131 132 155 154
		f 4 -108 149 166 -157
		mu 0 4 132 133 156 155
		f 4 -36 140 167 -154
		mu 0 4 109 107 158 157
		f 4 116 169 -171 -169
		mu 0 4 107 45 159 158
		f 4 119 174 -176 -173
		mu 0 4 45 30 160 159
		f 4 -122 172 178 -178
		mu 0 4 113 110 162 161
		f 4 -125 180 182 -182
		mu 0 4 114 112 164 163
		f 4 125 177 -184 -181
		mu 0 4 5 38 166 165
		f 4 127 185 -187 -175
		mu 0 4 38 114 163 166
		f 4 -130 187 188 -186
		mu 0 4 111 108 168 167
		f 4 -131 168 189 -188
		mu 0 4 23 49 140 169
		f 4 -134 190 192 -192
		mu 0 4 134 135 171 170
		f 4 135 193 -195 -191
		mu 0 4 135 136 172 171
		f 4 136 181 -196 -194
		mu 0 4 136 137 173 172
		f 4 -138 191 197 -170
		mu 0 4 138 139 175 174;
	setAttr ".cd" -type "dataPolyComponent" Index_Data Edge 0 ;
	setAttr ".cvd" -type "dataPolyComponent" Index_Data Vertex 0 ;
	setAttr ".hfd" -type "dataPolyComponent" Index_Data Face 0 ;
createNode groupId -n "groupId59";
	setAttr ".ihi" 0;
select -ne :time1;
	setAttr ".o" 1;
	setAttr ".unw" 1;
select -ne :renderPartition;
	setAttr -s 2 ".st";
select -ne :renderGlobalsList1;
select -ne :defaultShaderList1;
	setAttr -s 2 ".s";
select -ne :postProcessList1;
	setAttr -s 2 ".p";
select -ne :defaultRenderingList1;
select -ne :initialShadingGroup;
	setAttr -s 3 ".dsm";
	setAttr ".ro" yes;
	setAttr -s 3 ".gn";
select -ne :initialParticleSE;
	setAttr ".ro" yes;
select -ne :defaultResolution;
	setAttr ".pa" 1;
select -ne :hardwareRenderGlobals;
	setAttr ".ctrs" 256;
	setAttr ".btrs" 512;
select -ne :hardwareRenderingGlobals;
	setAttr ".otfna" -type "stringArray" 22 "NURBS Curves" "NURBS Surfaces" "Polygons" "Subdiv Surface" "Particles" "Particle Instance" "Fluids" "Strokes" "Image Planes" "UI" "Lights" "Cameras" "Locators" "Joints" "IK Handles" "Deformers" "Motion Trails" "Components" "Hair Systems" "Follicles" "Misc. UI" "Ornaments"  ;
	setAttr ".otfva" -type "Int32Array" 22 0 1 1 1 1 1
		 1 1 1 0 0 0 0 0 0 0 0 0
		 0 0 0 0 ;
select -ne :defaultHardwareRenderGlobals;
	setAttr ".res" -type "string" "ntsc_4d 646 485 1.333";
select -ne :ikSystem;
	setAttr -s 4 ".sol";
connectAttr "groupId59.id" "Door_LG_02Shape.iog.og[0].gid";
connectAttr ":initialShadingGroup.mwc" "Door_LG_02Shape.iog.og[0].gco";
connectAttr "Door_LG_02Shape.iog.og[0]" ":initialShadingGroup.dsm" -na;
connectAttr "groupId59.msg" ":initialShadingGroup.gn" -na;
// End of Door_LG_02.ma
# AM-Radio-Pattern
Applictation to visulazie AM radio broadcast patterns.
Inital code written in 2012 tagged "2012"
Initially written in a few hours as a test to see if I could learn the new UI api that fast.

# What it is; how it works

An array of antennas broadcasting AM Radio, can be configured to change its antenna pattern. Each antenna has four parameters: ratio, orientation, spacing, and phase. ratio is 0..1, and all other are in degreess from -360..360
Ratio, and phase can be changed after the array is built. The number, spacing and orientation cannot be easilly changed after the array is built.

Upon start up the default problem from the text book is entered; this was a quick test to ensure the math was correct.

As each parameter is varied the pattern changes.
The map of towers, and the pattern are at two very differnt scales, at the scale of the pattern all the antennas are at the origin. The towers are scaled up to show their relative positions.

I intend to brush up on my C# by reforing this, and adding a few quality of life features. It is useable now if any radio engineers need it, but it will get better.

# how to build

AMPattern.sln can be opened by visual studo comunity edition, I last used VS 2022.
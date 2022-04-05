# LottoGenerator
This API code generates sets of numbers that can be used for lottery picks, among other things.
It takes advantage of CPU noise as a source for randomness (1 or 0), builds digits, bit by random bit, into an integer.
The difference here being that it provides better random numbers than most other applications.
(this was tested thoroughly with a quad core CPU for randomness over billions of iterations and bias was > 2 orders of magnitude better than any pseudo random number generator available.
Regarding lottery numbers: 
There are 2 things (besides number ranges and set size) that can be tweaked when making a request set: 1) The odd/even ratio and 2) The mean sum (for 2 or more numbers)
This would, in theory, increase the odds of a winning set. 

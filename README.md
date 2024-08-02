# BE-Interview


## Getting started

1. Clone the repository.

```
git clone https://stan-indoorlab@gitlab.com/askboss/blattnertech-subsidaries/the-indoor-lab/be-interview.git
```

2. Change directory to the project.
```
cd be-interview/
```

3. Install dependencies and run.
```
dotnet run
```

## Problem Description

**Situation**: 
<br />
The current code base will connect to a websocket and receive a `Trackframe` every second. A `Trackframe` is a representation of objects and their position at specific moment of time. The `Trackframe` object have a list of object's coordinates/id's, and a timestamp. The websocket will emit 2 minutes worth of frames(120 `Trackframes`) before repeating itself.
<br />
<br />
There is also a `Region` object where a list coordinates represents a corner of a square.

**Assignment**:
<br />
For every `Trackframe` that is emitted, we would like to know a cumulative number of objects that have entered the square `Region`. Display this value every second.
<br />
<br />
***Ex.***
If 4 objects entered the square in the first second and 10 objects entered the squre in the next second. The output should look like this
```
4
14
```


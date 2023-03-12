# StarWars.JediArchives

This is the Backend of my Star Wars universe. It will feed any client of my related frontend applications in my ecosystem.
This has been created basically from two main purposes:
1. Having fun coding, designing new feautures in Clean Architecture style
2. Digging deeper into technologies and solving ongoing problems

The first one is clear but the second is more complex. My intention is to solve existing problems in my own way and than compare them to existing solution.
It will be interesting and I think more effective after realizing problems in depth and designing working solutions to understand someone else's existing solutions.
I will create POCs here and maybe it will be grown as separated side projects. If it will be good enough I will consider to publish them as a free licensed nuget package.

# Side Project: URL query parameter handling

## Problem
If you have an endpoint on backend, sometimes you need to introduce query parameters. This sounds very easy, because the major problems are handled by the framework. In a default project you don't have to take care of parsing, which can be customized e.g using IParsable<T> implementations for annotated items. Thanks to this solution you are able to parse all parameter you need to process.
But there is a problem here. If you extend the number of possible parameters you have to extend your parsers or extend the existing bindign mechanism. And finally if the data is already there in an appropriate format, you have to design a pattern how each query paramater should be handled. Meaning: Where your query peaces are together somewhere in a lower layer of your application where the database will be querried you always have to maintain and take care of changes. And somethimes it would be useful to validate data somewhere else closer to the incoming paramaters.

## The Idea
I would like to develop a mechanism which can help in the following:
* defining rules in central way
* defining rules in human eye readible manner
* Evaluating parameters as early as it is possible
* Generating meaningful validation errors
* Query parameters should be executed in an order which was intended by the user at frontend
* Query by frontend developer can contain 'n' differnt parameters 
* the output should feed LINQ queries
* the output should handle with Compare kind queries and OrderBy kind queries

## Real-life developer example
As a backend developer you have the following url:
```bash
/api/v1/timeline/all?query=startYear[gte]=-100&End_Year[lte]=10&ranklevel[qt]=10
```
* 'timeline/all' represents the endpoint in your Timeline Controller where someone/or you :) at the frontend want to get all timelines from.
* 'query' represents the name of the single item which contains the whole infinite query string
*  'startYear[gte]=-100' represents a query item. The frontend expects this will be examined first and means all greater than -100 value based on StartYear property of Timeline VM is needed. Something like this:
```C#
return timelines.Where(tl => tl.StartYear > -100);  
```
And based on this analogue for the whole query this result chain is expected:
```C#
return timelines
  .Where(tl => tl.StartYear > -100)
  .Where(tl => tl.EndYear < 10)
  .Where(tl => tl.RankLevel == 10);
```
If you take a look at the query you will realise that properties are coming not the same format as we are naming the properties. Okay I accept this is pretty rare that the same url contains more formats. But the point is here to provide a flexible solution for handling all of them.
  
### Your Needs as a developer
Oh dude, what a data sausage! I have to deal with the following problems:
* introduce a banch of parameters to the http method
* decide that parameters are fitting with my parser and finally with my model/record/dto/vm
* validate the types
* parsing the meaning of operations
* find a solution to transfer data between components
* and find a solution to handle special requerements which was expressed in query string
* introduce some meaningful errors in case of invalid input data
* and I have to take care of this whole mechanism controller by controller.. and worst view model by view model
  
Don't worry! This is the problem which I have worked on. It is not ready yet but I am showing what I have already: 

# How to start
In Startup.cs explicitly or implicitly (in any extension where IServiceCollection registraion happens). As a result of this registration you got the following:
* You can easily define parsing rules
* This rules are Regex based human readible ones
* You will have as many QueryProcessor as Dto/record/model/vm you need to work with from this perspective. This is typically a List kind Dto. TimeListDto in my example.
* Since it is a service, you will be able to access it via DI, IOC container
  
You can define your own rules this way:
```C#
  services.AddQueryProcessor<TimelineListDto>( builder =>
            {
                builder
                .WithNewFilteredRule(@"(\[gte\])")
                .WithValueFromCharacterUntilEndIndex('=', 0)
                .WithPropertyFromIndexUntilEndCharacter(0, '[')
                .WithExpectedComparer(Comparer.Greater);
            });
```
As you can see, you can add my service ```C# QueryProcessor<T> ``` via using my builder ```C# QueryProcessorStatedBuilder<T> ``` to create any rule.
Thanks to this, the Regex based rule will be applied when query string is coming. And if it fits than you have to configure where the value and property name can be found. Than the property will be cleaned from any unnecessary characher (eg. '_') than it will be resolved as a true property /this will happen in case insensitive way/. You also define here, that this shape of parameter will be processed as Comparer /not OrderBy/ operation; Especially as a Greater comparer..
  
Oh.. Sounds good but.. is it good for only one parameter kind? You're right, than let's see and do this:
```C#
  services.AddQueryProcessor<TimelineListDto>( builder =>
            {
                builder
                .WithNewFilteredRule(@"(\[gte\])")
                .WithValueFromCharacterUntilEndIndex('=', 0)
                .WithPropertyFromIndexUntilEndCharacter(0, '[')
                .WithExpectedComparer(Comparer.Greater)

                .WithNewFilteredRule(@"(\[lte\])")
                .WithValueFromCharacterUntilEndIndex('=', 0)
                .WithPropertyFromIndexUntilEndCharacter(0, '[')
                .WithExpectedComparer(Comparer.Greater)

                .WithNewFilteredRule(@"(\[qt\])")
                .WithValueFromCharacterUntilEndIndex('=', 0)
                .WithPropertyFromIndexUntilEndCharacter(0, '[')
                .WithExpectedComparer(Comparer.Greater);
            });
```
Now you have 3 different rules for one model/vm/record/dto. But do you need more than one?
No problem. I created this in generic way, so you just have to say to the compiler that which type should be resolved for.  
So long story short as many Processor Service will be registered as many type you has been configured:
```C#
  new ServiceDescriptor(typeof(IQueryProcessor<T>), queryProcessor)
```
  
So you can register more rules and more types this way:
```C#
  services.AddQueryProcessor<TimelineListDto>( builder =>
            {
                builder
                .WithNewFilteredRule(@"(\[gte\])")
                .WithValueFromCharacterUntilEndIndex('=', 0)
                .WithPropertyFromIndexUntilEndCharacter(0, '[')
                .WithExpectedComparer(Comparer.Greater)

                .WithNewFilteredRule(@"(\[lte\])")
                .WithValueFromCharacterUntilEndIndex('=', 0)
                .WithPropertyFromIndexUntilEndCharacter(0, '[')
                .WithExpectedComparer(Comparer.Greater)

                .WithNewFilteredRule(@"(\[qt\])")
                .WithValueFromCharacterUntilEndIndex('=', 0)
                .WithPropertyFromIndexUntilEndCharacter(0, '[')
                .WithExpectedComparer(Comparer.Greater);
            });

            services.AddQueryProcessor<CharacterListDto>(builder =>
            {
                builder
                .WithNewFilteredRule(@"(\[gte\])")
                .WithValueFromCharacterUntilEndIndex('=', 0)
                .WithPropertyFromIndexUntilEndCharacter(0, '[')
                .WithExpectedComparer(Comparer.Greater)

                .WithNewFilteredRule(@"(\[lte\])")
                .WithValueFromCharacterUntilEndIndex('=', 0)
                .WithPropertyFromIndexUntilEndCharacter(0, '[')
                .WithExpectedComparer(Comparer.Greater)

                .WithNewFilteredRule(@"(\[qt\])")
                .WithValueFromCharacterUntilEndIndex('=', 0)
                .WithPropertyFromIndexUntilEndCharacter(0, '[')
                .WithExpectedComparer(Comparer.Greater);
            });
```

Hm.. okay, okay but where are the OrderBy Operation? And what about any other definition type?
You caught me! This is a limitation for now beacuse I am too busy at work. But I will continue work with this.
Please see the `Actual limitations` section!
  
# How to Process/Validate
You have to call the registered Processor. Thanks to DI the services are already there:
```C#
        [HttpGet("all", Name = "GetAllTimelines")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<List<TimelineListDto>>> GetAllTimelines(int? page, int? size, string query)
        {
            _queryProcessor.Run(query);
```
Of yourse it shouldn't be called in the Controller directly but it is just for demonstration purposes.
The run will build you ruleset which will be applicable for you actual incoming query. This is because delegate methods are used in order to examined with the actual data.
  
# How to work with outputs
To achive the goal you must meet a prerequisite. Don't worry, it is just calling the Run(). Described in `How to Process/Validate` section.
In this phase I am just putting here a code snippet from one of my Test method:
```C#
            var enumerator = _queryProcessor.ExecutableEnumerator;

            IEnumerable<dynamic> resultList = compareByValues;

            while(enumerator.MoveNext())
            {
                var queryOperation = enumerator.Current.CompareTask;
                var tempResultList = resultList.Where(queryOperation).Where(a => a.S ==0 ).Where(a=>a.d >2);
                resultList = tempResultList;
            }
```
I was thinkig a lot on inmutability so I choosed a way to make this Process collection accessible as an enumerator.
And than the result is that queries are executed in the order how it was stated in the query string.
  
# Actual limitations
As far as I mentioned I have lot of things to do, so I this solution has a lot of limitations:
* It hasn't been a standalone solution or package YET
* This is not a shiny framework or workflow based mechanism, just some classes and lines FOR NOW
* It handles only the following Compare Operations: '>', '<', '==';
* It hasn't supported OrderBy kind YET
* It only works with 'int32' Properties. FOR NOW
* There are limited options to define how the paramter looks like after it is recognized based on reex rule. But actual implementation good enough for most cases.
  So you only have this two methods:
  ```C#
    WithValueFromCharacterUntilEndIndex(char charFrom, int indexFromEnd)
    WithPropertyFromIndexUntilEndCharacter(int fromIndex, char untilEndChar)
  ```
* There are a lot of unwritten test :) YET

Please feel free to use it but don't forget it is my pet project which is under development. Please don't use it without life insurance ;) -just kiddin

## Legal Notes or something like that 
                                                             
Side notes for the Title and Repository name:
Star Wars and Jedi are trademarked expressions.
Creating a product from this code is NOT intended I just love to use Star Wars related names when practicing or learning something.
It has been made comfortable and fun. So if you are disappointed and need real Star Wars content, than visit this site:
[Star Wars Official Home Page](https://www.starwars.com/)


var camelCaseTokenizer = function (builder) {

  var pipelineFunction = function (token) {
    var previous = '';
    // split camelCaseString to on each word and combined words
    // e.g. camelCaseTokenizer -> ['camel', 'case', 'camelcase', 'tokenizer', 'camelcasetokenizer']
    var tokenStrings = token.toString().trim().split(/[\s\-]+|(?=[A-Z])/).reduce(function(acc, cur) {
      var current = cur.toLowerCase();
      if (acc.length === 0) {
        previous = current;
        return acc.concat(current);
      }
      previous = previous.concat(current);
      return acc.concat([current, previous]);
    }, []);

    // return token for each string
    // will copy any metadata on input token
    return tokenStrings.map(function(tokenString) {
      return token.clone(function(str) {
        return tokenString;
      })
    });
  }

  lunr.Pipeline.registerFunction(pipelineFunction, 'camelCaseTokenizer')

  builder.pipeline.before(lunr.stemmer, pipelineFunction)
}
var searchModule = function() {
    var documents = [];
    var idMap = [];
    function a(a,b) { 
        documents.push(a);
        idMap.push(b); 
    }

    a(
        {
            id:0,
            title:"ReSharperReportsSettings",
            content:"ReSharperReportsSettings",
            description:'',
            tags:''
        },
        {
            url:'/Cake.ReSharperReports/api/Cake.ReSharperReports/ReSharperReportsSettings',
            title:"ReSharperReportsSettings",
            description:""
        }
    );
    a(
        {
            id:1,
            title:"ReSharperReportsRunner",
            content:"ReSharperReportsRunner",
            description:'',
            tags:''
        },
        {
            url:'/Cake.ReSharperReports/api/Cake.ReSharperReports/ReSharperReportsRunner',
            title:"ReSharperReportsRunner",
            description:""
        }
    );
    a(
        {
            id:2,
            title:"ReSharperReportsAliases",
            content:"ReSharperReportsAliases",
            description:'',
            tags:''
        },
        {
            url:'/Cake.ReSharperReports/api/Cake.ReSharperReports/ReSharperReportsAliases',
            title:"ReSharperReportsAliases",
            description:""
        }
    );
    var idx = lunr(function() {
        this.field('title');
        this.field('content');
        this.field('description');
        this.field('tags');
        this.ref('id');
        this.use(camelCaseTokenizer);

        this.pipeline.remove(lunr.stopWordFilter);
        this.pipeline.remove(lunr.stemmer);
        documents.forEach(function (doc) { this.add(doc) }, this)
    });

    return {
        search: function(q) {
            return idx.search(q).map(function(i) {
                return idMap[i.ref];
            });
        }
    };
}();

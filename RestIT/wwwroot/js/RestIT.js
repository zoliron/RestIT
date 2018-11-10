$(document).ready(function () {
    $("button").click(function () {
        $("#RestaurantAdvancedSearch").fadeToggle('slow');
    });
});

$(document).ready(function () {
    $("button").click(function () {
        $("#DishAdvancedSearch").fadeToggle('slow');
    });
});

$(document).ready(function () {
    $("button").click(function () {
        $("#CustomerAdvancedSearch").fadeToggle('slow');
    });
});

function restaurantCitiesChart() {
    var data = document.getElementById('restaurantCities').innerText;

    data = data.split(",");
    var CitiesNumber = data.length / 2;
    var Cities = "";
    var CitiesCount = "";

    for (i = 0; i < data.length - 1; i += 2) {
        Cities += data[i];
        Cities += ",";
        CitiesCount += data[i + 1];
        CitiesCount += ",";
    }

    Cities = Cities.split(",");
    CitiesCount = CitiesCount.split(",");

    var svg = d3.select("svg[name='countRestaurants']"),
        margin = { top: 20, right: 20, bottom: 30, left: 40 },
        width = +svg.attr("width") - margin.left - margin.right,
        height = +svg.attr("height") - margin.top - margin.bottom;

    var x = d3.scaleBand().rangeRound([0, width]).padding(0.3),
        y = d3.scaleLinear().rangeRound([height, 0]);

    var g = svg.append("g")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

    var d = [{ "City": "", "count": "" }]

    for (i = 0; i < CitiesNumber; i++) {
        d.push({ "City": Cities[i], "count": CitiesCount[i] });
    }

    x.domain(d.map(function (d) { return d.City; }));
    y.domain([0, d3.max(d, function (d) { return d.count; })]);

    g.append("g")
        .attr("class", "axis axis--x")
        .attr("transform", "translate(0," + height + ")")
        .call(d3.axisBottom(x));

    g.append("g")
        .attr("class", "axis axis--y")
        .call(d3.axisLeft(y).ticks(10))
        .append("text")
        .attr("transform", "rotate(-90)")
        .attr("y", 6)
        .attr("dy", "0.71em")
        .attr("text-anchor", "end")
        .text("Restaurants Number");

    g.selectAll(".bar")
        .data(d)
        .enter().append("rect")
        .attr("class", "bar")
        .attr("x", function (d) { return x(d.City); })
        .attr("y", function (d) { return y(d.count); })
        .attr("width", x.bandwidth())
        .attr("height", function (d) { return height - y(d.count); });
}

function restaurantTypesChart() {
    var data = document.getElementById('restaurantTypes').innerText;

    data = data.split(",");
    var TypesNumber = data.length / 2;
    var Types = "";
    var TypesCount = "";

    for (i = 0; i < data.length - 1; i += 2) {
        Types += data[i];
        Types += ",";
        TypesCount += data[i + 1];
        TypesCount += ",";
    }

    Types = Types.split(",");
    TypesCount = TypesCount.split(",");

    var svg = d3.select("svg[name='countRestaurants']"),
        margin = { top: 20, right: 20, bottom: 30, left: 40 },
        width = +svg.attr("width") - margin.left - margin.right,
        height = +svg.attr("height") - margin.top - margin.bottom;

    var x = d3.scaleBand().rangeRound([0, width]).padding(0.1),
        y = d3.scaleLinear().rangeRound([height, 0]);

    var g = svg.append("g")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

    var d = [{ "Type": "", "count": "" }]

    for (i = 0; i < TypesNumber; i++) {
        d.push({ "Type": Types[i], "count": TypesCount[i] });
    }

    x.domain(d.map(function (d) { return d.Type; }));
    y.domain([0, d3.max(d, function (d) { return d.count; })]);

    g.append("g")
        .attr("class", "axis axis--x")
        .attr("transform", "translate(0," + height + ")")
        .call(d3.axisBottom(x));

    g.append("g")
        .attr("class", "axis axis--y")
        .call(d3.axisLeft(y).ticks(10))
        .append("text")
        .attr("transform", "rotate(-90)")
        .attr("y", 6)
        .attr("dy", "0.71em")
        .attr("text-anchor", "end")
        .text("Restaurants Number");

    g.selectAll(".bar")
        .data(d)
        .enter().append("rect")
        .attr("class", "bar")
        .attr("x", function (d) { return x(d.Type); })
        .attr("y", function (d) { return y(d.count); })
        .attr("width", x.bandwidth())
        .attr("height", function (d) { return height - y(d.count); });
}



Parse.Cloud.define("match", function(request, response) {

	var now = new Date();
	var today = new Date(now.getFullYear(), now.getMonth(), now.getDate());
	var yesterday = new Date(now.getFullYear(), now.getMonth(), now.getDate());
	yesterday.setDate(today.getDate() - 1);
  
	var fromDate;
	var toDate;
	var dateField = "endDate";
	
	var page = 1;
	var pageSize = 100;
	if (request.params.page != undefined && !isNaN(request.params.page) && request.params.page > 0) {
		page = Math.floor(request.params.page);
	}
	if (request.params.pageSize != undefined && !isNaN(request.params.pageSize) && request.params.pageSize <= 100 && request.params.pageSize > 0) {
		pageSize = Math.floor(request.params.pageSize);
	}

	if (request.params.date == "today") {
		fromDate = new Date(today.getFullYear(), today.getMonth(), today.getDate());
		toDate = new Date(today.getFullYear(), today.getMonth(), today.getDate());
		toDate.setDate(fromDate.getDate() + 1);
	} else if (request.params.date == "yesterday") {
		fromDate = new Date(yesterday.getFullYear(), yesterday.getMonth(), yesterday.getDate());
		toDate = new Date(today.getFullYear(), today.getMonth(), today.getDate());
		toDate.setDate(fromDate.getDate() + 1);
	} else if (request.params.date == "older") {
		toDate = new Date(yesterday.getFullYear(), yesterday.getMonth(), yesterday.getDate());
		fromDate = new Date(today.getFullYear(), today.getMonth(), today.getDate());
		fromDate.setDate(toDate.getDate() - 14);
	} else {
		fromDate = new Date(today.getFullYear(), today.getMonth(), today.getDate());
		fromDate.setDate(fromDate.getDate() - 14);
		toDate = new Date(today.getFullYear(), today.getMonth(), today.getDate());
		toDate.setDate(toDate.getDate() + 1);
		//dateField = "createdAt";
	}
	
	var ruleQuery = new Parse.Query("Rule");
	if (request.params.rule != undefined) {
		ruleQuery.equalTo("objectId", request.params.rule);
	} else {
		ruleQuery.equalTo("isActive", true);
	}

	ruleQuery.find().then(
		function(rules) {
			var compoundQuery;

			for (var i = 0 ; i < rules.length ; i++) {
				var rule = rules[i];
				var query = new Parse.Query("Sighting");
				
				var valid = false;
				if (rule.get("taxons")) {
					query.containedIn("taxonName", rule.get("taxons"));
					valid = true;
				}
				if (rule.get("kommuner")) {
					query.containedIn("kommun", rule.get("kommuner"));
					valid = true;
				}
				if (rule.get("landskap")) {
					query.containedIn("landskap", rule.get("landskap"));
					valid = true;
				}
				if (rule.get("prefix")) {
					query.lessThanOrEqualTo("taxonPrefix", rule.get("prefix"));
					valid = true;
				}

				if (valid) {
					if (!compoundQuery) {
						compoundQuery = query;
					} else {
						compoundQuery = Parse.Query.or(compoundQuery, query);
					}
				}
			}
			
			if (compoundQuery) {
				compoundQuery.descending("sightingId");
				if (page > 1) {
					compoundQuery.skip((page - 1) * pageSize);
				}
				compoundQuery.limit(pageSize);

				if (fromDate && toDate) {
					compoundQuery.greaterThanOrEqualTo(dateField, fromDate);
					compoundQuery.lessThan(dateField, toDate);
				}
				
				compoundQuery.find({
					success: function(results) {
						response.success(results);
					},
					error: function(error) {
						response.error("matching failed: " + error);
					}
				});
				
			} else {
				response.error("no active rules");
			}
		},
		function(error) {
		  response.error("rules query failed: " + error);
		}
	);

});

Parse.Cloud.beforeSave("Site", function(request, response) {
	var site = request.object;
	
	if (!site.id) {
		// this is a new object
		console.log("New site check for uniqueness");
		var query = new Parse.Query("Site");
			query.equalTo("siteId", site.get("siteId"));
			query.first({
			  success: function(oldSite) {
				if (oldSite) {
				  response.error("A Site with this siteId already exists.");
				} else {
				  response.success();
				}
			  },
			  error: function(error) {
				response.error("Could not validate uniqueness for this Site object.");
			  }
			});
    } else {
		response.success();
	}
});


Parse.Cloud.beforeSave("Sighting", function(request, response) {
	var sighting = request.object;
	
	if (!sighting.id) {
		// this is a new object
		console.log("New sighting check for uniqueness");
		var query = new Parse.Query("Sighting");
			query.equalTo("sightingId", sighting.get("sightingId"));
			query.first({
			  success: function(oldSighting) {
				if (oldSighting) {
				  response.error("A Sighting with this sightingId already exists.");
				} else {
				  response.success();
				}
			  },
			  error: function(error) {
				response.error("Could not validate uniqueness for this Sighting object.");
			  }
			});
    } else {
		response.success();
	}
});

Parse.Cloud.beforeSave("Taxon", function(request, response) {
	var taxon = request.object;
	
	if (!taxon.id) {
		// this is a new object
		console.log("New taxon check for uniqueness");
		var query = new Parse.Query("Taxon");
			query.equalTo("taxonId", taxon.get("taxonId"));
			query.first({
			  success: function(oldTaxon) {
				if (oldTaxon) {
				  response.error("A Taxon with this taxonId already exists.");
				} else {
				  response.success();
				}
			  },
			  error: function(error) {
				response.error("Could not validate uniqueness for this Taxon object.");
			  }
			});
    } else {
		response.success();
	}
});
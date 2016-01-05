Parse.Cloud.define("testDate", function(request, response) {

	var now = new Date();
	var today = new Date(now.getFullYear(), now.getMonth(), now.getDate());
	var yesterday = new Date(now.getFullYear(), now.getMonth(), now.getDate());
	yesterday.setDate(today.getDate() - 1);
  
	var fromDate;
	var toDate;
	
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
	}

	response.success({
		"fromDate" : fromDate,
		"toDate" : toDate
	});
});


Parse.Cloud.define("search", function(request, response) {

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
	
	var taxons = request.params.taxons;
	var kommuner = request.params.kommuner;
	var landskap = request.params.landskap;
	var prefix = request.params.prefix;
	var latitude = request.params.latitude;
	var longitude = request.params.longitude;	
		
	var query = new Parse.Query("Sighting");

	if (fromDate && toDate) {
		query.greaterThanOrEqualTo(dateField, fromDate);
		query.lessThan(dateField, toDate);
	}

	if (latitude && longitude) {
		var point = new Parse.GeoPoint({latitude: latitude, longitude: longitude});
		query.near("location", point);
	} else {
		query.descending("sightingId");
	}
	
	if (taxons) {
		query.containedIn("taxonName", taxons);
	}
	if (kommuner) {
		query.containedIn("kommun", kommuner);
	}
	if (landskap) {
		query.containedIn("landskap", landskap);
	}
	if (prefix && prefix != 9) {
		query.lessThanOrEqualTo("taxonPrefix", prefix);
	}
	
	if (page > 1) {
		query.skip((page - 1) * pageSize);
	}
	query.limit(pageSize);
	
	query.find({
		success: function(results) {
			response.success(results);
		},
		error: function(error) {
			response.error("search failed: " + error.code);
		}
	});

});


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
				if (rule.get("prefix") && rule.get("prefix") != 9) {
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
	var siteId = site.get("siteId");
	
	if (!site.id && siteId != null) {
		// this is a new object
		console.log("New site check for uniqueness");
		var query = new Parse.Query("Site");
			query.equalTo("siteId", site.get("siteId"));
			query.first({
			  success: function(oldSite) {
				if (oldSite) {
				  response.error("A Site with this siteId already exists.");
				}
			  },
			  error: function(error) {
				response.error("Could not validate uniqueness for this Site object.");
			  }
			});
	}
	
	var search = [];
	var siteName = (site.get("siteName") || "").toLowerCase().replace(/[^a-zåäö\d]/gi, " ");
	var superSite = (site.get("superSite") || "").toLowerCase();
	var kommun = (site.get("kommun") || "").toLowerCase();
	var landskap = (site.get("landskap") || "").toLowerCase();
	if (siteName != "") search.push(siteName);
	if (superSite != "") search.push(superSite);
	if (kommun != "") search.push(kommun);
	if (landskap != "") search.push(landskap);
	
	site.set("search", search.join(" "));
    response.success();
});


Parse.Cloud.beforeSave("Sighting", function(request, response) {
	var sighting = request.object;
	var sightingId = sighting.get("sightingId");
	if (!sighting.id && sightingId != null) {
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

Parse.Cloud.define("matchRules", function(request, response) {

	Parse.Cloud.useMasterKey();
	if (!request.params.id) {
		response.error("no sighting id supplied");
	}


	var ParseSighting = Parse.Object.extend("Sighting");
	var query = new Parse.Query(ParseSighting);
	query.get(request.params.id, {
	  success: function(sighting) {
		// The object was retrieved successfully.

		
		var ruleQuery = new Parse.Query("Rule");
		ruleQuery.equalTo("isActive", true);
		ruleQuery.greaterThanOrEqualTo("prefix", sighting.get("taxonPrefix"));
		ruleQuery.containedIn("taxons", [sighting.get("taxonName"), null]);
		ruleQuery.containedIn("kommuner", [sighting.get("kommun"), null]);
		ruleQuery.containedIn("landskap", [sighting.get("landskap"), null]);
		
		ruleQuery.find({
			success: function(rules) {
				response.success(rules);
			},
			error: function(error) {
				response.error("matching failed: " + error);
			}
		});
		
	  },
	  error: function(object, error) {
		// The object was not retrieved successfully.
		// error is a Parse.Error with an error code and message.
		response.error(error);
	  }
	});

});


Parse.Cloud.afterSave("Sighting", function(request, response) {

	Parse.Cloud.useMasterKey();

	request.object.fetch({
	  success: function(sighting) {

		var ruleQuery = new Parse.Query("Rule");
		ruleQuery.equalTo("isActive", true);
		ruleQuery.greaterThanOrEqualTo("prefix", sighting.get("taxonPrefix"));
		ruleQuery.containedIn("taxons", [sighting.get("taxonName"), null]);
		ruleQuery.containedIn("kommuner", [sighting.get("kommun"), null]);
		ruleQuery.containedIn("landskap", [sighting.get("landskap"), null]);
		
		ruleQuery.find({
			success: function(rules) {

				if (rules.length > 0) {
					var taxonName = sighting.get("taxonName");
					var kommun = sighting.get("kommun");
					var landskap = sighting.get("landskap");
					var endDate = sighting.get("endDate");
					var observer = sighting.get("sightingObservers");
					var landskapShort = landskap;
					
					if (observer.indexOf(",") != -1) {
						observer = observer.substr(0, observer.indexOf(","));
					}

					var timeString = endDate.toTimeString().split(' ')[0];
					if (timeString.indexOf(":") != -1) {
						timeString = timeString.substr(0, timeString.lastIndexOf(":"));
					}
					var dateString = endDate.getDate() + "/" + (endDate.getMonth() + 1);
					if (timeString != "00:00") {
						dateString = dateString + " " + timeString;
					}
					
					switch (landskap) {
					  case "Skåne": landskapShort = "Sk"; break;
					  case "Blekinge": landskapShort = "Bl"; break;
					  case "Småland": landskapShort = "Sm"; break;
					  case "Öland": landskapShort = "Öl"; break;
					  case "Gotland": landskapShort = "Go"; break;
					  case "Halland": landskapShort = "Hl"; break;
					  case "Bohuslän": landskapShort = "Bo"; break;
					  case "Dalsland": landskapShort = "Ds"; break;
					  case "Västergötland": landskapShort = "Vg"; break;
					  case "Närke": landskapShort = "Nä"; break;
					  case "Östergötland": landskapShort = "Ög"; break;
					  case "Södermanland": landskapShort = "Sö"; break;
					  case "Uppland": landskapShort = "Up"; break;
					  case "Västmanland": landskapShort = "Vs"; break;
					  case "Värmland": landskapShort = "Vr"; break;
					  case "Dalarna": landskapShort = "Da"; break;
					  case "Gästrikland": landskapShort = "Gä"; break;
					  case "Medelpad": landskapShort = "Me"; break;
					  case "Hälsingland": landskapShort = "Hs"; break;
					  case "Ångermanland": landskapShort = "Ån"; break;
					  case "Västerbotten": landskapShort = "Vb"; break;
					  case "Norrbotten": landskapShort = "Nb"; break;
					  case "Härjedalen": landskapShort = "Hä"; break;
					  case "Jämtland": landskapShort = "Jä"; break;
					  case "Åsele lappmark": landskapShort = "Ås"; break;
					  case "Lycksele lappmark": landskapShort = "Ly"; break;
					  case "Pite lappmark": landskapShort = "Pi"; break;
					  case "Lule lappmark": landskapShort = "Lu"; break;
					  case "Torne lappmark": landskapShort = "To"; break;
					}				

					var query = new Parse.Query("Alert");
					query.equalTo("taxonName", taxonName);
					query.equalTo("kommun", kommun);
					query.equalTo("landskap", landskap);
					query.greaterThanOrEqualTo("endDate", endDate);

					query.find().then(
						function(alerts) {
							if (alerts.length == 0) {
								
								var ParseAlert = Parse.Object.extend("Alert");
								var parseAlert = new ParseAlert();
								
								var pushMessage = taxonName + " " + kommun + ", " + landskapShort + " " + dateString + " (" + observer + ")";

								parseAlert.save({
								  taxonPrefix: sighting.get("taxonPrefix"),
								  taxonName: sighting.get("taxonName"),
								  kommun: sighting.get("kommun"),
								  landskap: sighting.get("landskap"),
								  endDate: sighting.get("endDate"),
								  pushMessage: pushMessage
								}, {
								  success: function(parseAlert) {
									// The object was saved successfully.
								  },
								  error: function(parseAlert, error) {
									// The save failed.
									// error is a Parse.Error with an error code and message.
									console.log("ALERT: Failed to save alert" + error);
								  }
								});
								
							}
						},
						function(error) {
							console.log("afterSave sighting error occured: " + error);
						});
				
				}

			},
			error: function(error) {
				console.log("matching failed: " + error);
			}
		});
	  },
	  error: function(user, error) {
		console.log("error fetched: " + error);
	  }
	});  
});



Parse.Cloud.afterSave("Alert", function(request) {

	Parse.Cloud.useMasterKey();

	var alertObject = request.object;
	var taxonName = alertObject.get("taxonName");
	var kommun = alertObject.get("kommun");
	var landskap = alertObject.get("landskap");
	var endDate = alertObject.get("endDate");
	var pushMessage = alertObject.get("pushMessage");
	
	var ruleQuery = new Parse.Query("Rule");
	ruleQuery.equalTo("isActive", true);
	ruleQuery.greaterThanOrEqualTo("prefix", alertObject.get("taxonPrefix"));
	ruleQuery.containedIn("taxons", [alertObject.get("taxonName"), null]);
	ruleQuery.containedIn("kommuner", [alertObject.get("kommun"), null]);
	ruleQuery.containedIn("landskap", [alertObject.get("landskap"), null]);
	
	ruleQuery.find().then(function(rules) {
		
		var users = [];
		
		for(var i = 0 ; i < rules.length ; i++) {
			users.push(rules[i].get("user"));
		}

		return users;
		
	}).then(function(users) {

		var installationQuery = new Parse.Query(Parse.Installation);
		installationQuery.containedIn('user', users);
		
		Parse.Push.send({
			where: installationQuery,
			data: {
				alert: pushMessage,
				badge: "Increment",
				sound: "default",
				title: taxonName + ", " + kommun
			}
		}, {
			success: function () {
				console.log("Push notification was sent");
			},
			error: function (error) {
				console.log("An error occured while trying to send push message. Error message: " + error);
			}
		});
		
	});
	
});


Parse.Cloud.define("installations", function(request, response) {

	Parse.Cloud.useMasterKey();
	var installationQuery = new Parse.Query(Parse.Installation);
	installationQuery.equalTo('user', request.user);

	installationQuery.find({
		success: function(installations) {
			response.success(installations);
		},
		error: function(error) {
			response.error("error getting sessions " + error);
		}
	});
});


Parse.Cloud.job("cleanUpOldSightings", function(request, status) {
	Parse.Cloud.useMasterKey();
	var counter = 0;

	var now = new Date();
	var date = new Date(now.getFullYear(), now.getMonth(), now.getDate());
	date.setDate(date.getDate() - 20);

	var query = new Parse.Query("Sighting");
	query.lessThan("endDate", date);
    
	status.message("Removing old sightings before: " + date);
		
	query.each(function(sighting) {
		if (counter % 100 === 0) {
			// Set the  job's progress status
			status.message(counter + " sightings processed.");
		}
		counter += 1;
		return sighting.destroy();
	}).then(function() {
		// Set the job's success status
		status.success(counter + " sigtings removed.");
	}, function(error) {
		// Set the job's error status
		status.error("Uh oh, something went wrong.");
	});
});

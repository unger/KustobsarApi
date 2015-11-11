
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
(function () {

    //Cache the DOM
    var body = $('body');

    //Cache the parent links that has sub-links on the menu
    var homeButtonLink = body.find('div > div > div > header.site-header > div.site-logo > a');

    // DashBoard Link
    var dashboardLink = body.find('div > div > div > ul#side-nav > li#dashboard > a');
    //Cache the parent links that has sub-links on the menu
    var sidebarParentLink = body.find('div > div >ul#side-nav > li.has-sub > a');

    //Cache the sub-links
    var sidebarChildLink = body.find('div > div >ul#side-nav > li.has-sub > ul.nav > li > a');


    //TODO: Placing parent link text on the page breadcrumb 
    sidebarParentLink.click(function (e) {
        var activeParentLink = $(this).find('span').text();
        body.find('#activeParentLink').text(activeParentLink);
        body.find('#activeParentHeader').text(activeParentLink);
    });


    /*
        TODO: Placing sub-link text on the page breadcrumb and 
        getting the html content for that page dynamically base
        on the url attribute.
    */
    sidebarChildLink.click(function (e) {
        e.preventDefault();
        dashboardLink.removeClass('active');

        var activeChildLink = $(this).text();

        var mainContent = body.find('div.main-content');
        var dynamicContent = body.find('div.dynamic-content');
        var url = $(this).attr('href');

        $("#page-title").text(activeChildLink);

        //body.find('.page-header').addClass('adjust-margin-bottom');

        $.ajax({
            url: url,
            dataType: "html",
            data: { IsFromSideMenu: true, Path: url },
            beforeSend: function () {
                dynamicContent.empty();
                dynamicContent.html('<img id="loader-img" alt="" src="images/spinner.gif" style="position: absolute; right: 50%; top: 30vh;" />');
            },
            success: function (data) {
                $('div.dynamic-content').html(data).addClass('auto-height');
            }
        });
    })

    // Home Link
    homeButtonLink.click(function (e) {
        e.preventDefault();
        body.find('div.main-content').addClass('active');
        var url = $(this).attr('href');
        $.ajax({
            url: url,
            dataType: "html",
            beforeSend: function () {
                $('div.main-content').empty().removeClass('auto-height');
                $('div.main-content').html('<img id="loader-img" alt="" src="images/spinner.gif.gif" width="50" height="50" style="position: absolute; right: 41%; top: 55vh;" />');
            },
            success: function (data) {
                $('div.main-content').html(data).addClass('auto-height');
            }
        });
    })

    // DashBoard Link
    dashboardLink.click(function (e) {
        e.preventDefault();

        sidebarChildLink.removeClass('active');
        dashboardLink.addClass('active');

        $("#page-title").text($(this).find('span').text());

        var activeChildLink = "";
        body.find('#activeChildLink').text(activeChildLink);
        body.find('main').addClass('active');
        
        var url = $(this).attr('href');

        $.ajax({
            url: url,
            dataType: "html",
            beforeSend: function () {
                $('main').empty().removeClass('auto-height');
                $('main').html('<img id="loader-img" alt="" src="images/spinner.gif" width="50" height="50" style="position: absolute; right: 41%; top: 55vh;" />');
            },
            success: function (data) {
                $('main').html(data).addClass('auto-height');
            }
        });
    })
}())
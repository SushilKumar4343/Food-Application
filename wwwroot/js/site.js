// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let apiURL = "https://forkify-api.herokuapp.com/api/v2/recipes";
let apikey = "733d5819-6f1a-4bec-85e9-b10689e4f08a";
async function GetRecipes(recipeName, id, isAllShow) {
    let resp = await fetch(`${apiURL}?search=${recipeName}&key=${apikey}`);
    let result = await resp.json();
    let Recipes = isAllShow ? result.data.recipes : result.data.recipes.slice(1, 7);
    showRecipes(Recipes, id);
}
function showRecipes(recipes, id) {
    $.ajax({
        contentType: "application/json; charset=utf-8",
        dataType: 'html',
        type: 'POST',
        url: '/Recipe/GetRecipeCard',
        data: JSON.stringify(recipes),
        success: function (htmlResult) {
            $('#' + id).html(htmlResult);
            getAddedCarts();
        }
    });
}

async function getOrderRecipe(id,showId) {
    let resp = await fetch(`${apiURL}/${id}?key=${apikey}`);
    let result = await resp.json();
    let recipe = result.data.recipe;
    showOrderRecipeDetials(recipe, showId);
}
function showOrderRecipeDetials(orderRecipeDetails, showId) {
    $.ajax({        
        url: '/Recipe/ShowOrder',
        data: orderRecipeDetails,
        dataType: 'html',
        type: 'POST',
        success: function (htmlResult) {
            $('#' + showId).html(htmlResult);
        }
    });
}

//order page
function quantity(option) {
    
    let qty = $('#qty').val();
    let price = parseInt($('#price').val());
    let totalAmount = 0;
    if (option === 'inc') {
        qty = parseInt(qty) + 1;
    } else {
        qty = qty == 1 ? qty : qty - 1;
    }
    totalAmount = price * qty;
    $('#qty').val(qty);
    $('#totalAmount').val(totalAmount);
}
// add to cart
async function cart() {
    let iTag = $(this).children('i')[0];
    let recipeId = $(this).attr('data-recipeId'); // Attribute names are case-sensitive
    if ($(iTag).hasClass('fa-regular')) {
        let resp = await fetch(`${apiURL}/${recipeId}?key=${apikey}`);
        let result = await resp.json();
        let cart = result.data.recipe;
        cart.recipeId = recipeId;
        delete cart.id;
        cartRequest(cart, 'SaveCart', 'fa-solid', 'fa-regular', iTag,false);
    } else {
        let data = { Id: recipeId };
        cartRequest(data, 'RemoveCartFromList', 'fa-solid', iTag,false)
    }
}
function cartRequest(data, action,addcls,removecls,iTag,isReload) {
    $.ajax({
        url: '/Cart/' + action,
        type: 'POST', // 'POST' should be a string
        data: data,
        success: function (resp) {
            if (isReload) {
                location.reload();
            } else {
                $(iTag).addClass(addcls);
                $(iTag).removeClass(removecls);
            }            
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function getAddedCarts() {
    $.ajax({
        url: '/Cart/GetAddedCarts',
        type: 'GET',
        dataType: 'json',
        success: function (result) {
            $('.addToCartIcon').each((index, spanTag) => { // Corrected selector
                let recipeId = $(spanTag).attr("data-recipeid"); // Ensure case sensitivity
                for (let i = 0; i < result.length; i++) {
                    if (recipeId == result[i]) {
                        let itag = $(spanTag).children('i')[0];
                        $(itag).addClass('fa-solid');
                        $(itag).removeClass('fa-regular');
                        break;
                    }
                }
            });
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function getCartList() {
    $.ajax({
        url: '/Cart/GetCartList',
        type: 'GET',
        dataType: 'html',
        success: function (result) {
            $('#showCartList').html(result);
        },
        error: function (err) {
            console.log(err);
        }
    });
}

function removeCartfromlist(id) {
    console.log();
    let data = { Id: id };
    cartRequest(data,'RemoveCartFromList', null, null, null, true)
}


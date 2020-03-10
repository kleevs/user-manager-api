(function (global, $) {
    $(function () {
        var $signinForm = $(`<div class='wrapper'>
                    <form class="col-10" style="display: inline-block;">
                        <div class="form-group row">
                            <label for="login" class="col-form-label" style="padding-left:20px;width:90px;font-size: 16px;color: gray;">Login :</label> 
                            <div class="col-3">
                                <input id="login" name="login" value="" type="text" class="form-control" required="required" />
                            </div>
                            <label for="password" class="col-form-label" style="width: 110px;font-size: 16px;color: gray;">Password :</label> 
                            <div class="col-3">
                                <input id="password" name="password" type="password" class="form-control" required="required" />
                            </div>
                            <div class="col-1" style="padding-top: 10px;">
                                <button name="submit" type="submit" class="btn btn-success" style="width:120px;color: #fff;background-color: #28a745;border-color: #28a745;/* padding: 7px; *//* margin: 0px; */">Connect</button>
                            </div>
                        </div>
                    </form>
                </div>`);

        $signinForm.submit(function (event) {
            var login = $("#login").val();
            var password = $("#password").val();
            new Promise(function (resolve, reject) {
                $.ajax({
                    type: 'POST',
                    url: "/accounts/login",
                    contentType: 'application/json',
                    data: JSON.stringify({
                        login: login,
                        password: password
                    })
                }).done(function (response) {
                    resolve(response);
                }).fail(function (error) {
                    reject(error);
                })
            });
            event.preventDefault();
        });
        setTimeout(function () {
            $(".information-container.wrapper").after($signinForm);
        }, 1000);
    });
})(window, $)
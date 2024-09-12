import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, GuardResult, MaybeAsync, Router, RouterStateSnapshot } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class AuthGuard implements CanActivate  {

    constructor(private router:Router, private jwtHelper: JwtHelperService){}

    canActivate() {
        const token = localStorage.getItem("id_token");

        if (token && !this.jwtHelper.isTokenExpired(token)){
            return true;
        }

        this.router.navigate(["login"]);
        return false;
    }
}    
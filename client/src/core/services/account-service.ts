import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { LoginCreds, RegisterCreds, User } from '../../types/user';
import { tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { LikesService } from './likes-service';

@Injectable({
    providedIn: 'root'
})
export class AccountService {
    private http = inject(HttpClient)
    private likeService = inject(LikesService);
    currentUser = signal<User | null>(null);
    private baseUrl = environment.apiUrl;

    register(creds: RegisterCreds){
        return this.http.post<User>(this.baseUrl + 'account/register', creds).pipe(
            tap(user => {
                this.setCurrentUser(user);
            })
        )
    }

    login(cred : LoginCreds){
        return this.http.post<User>(this.baseUrl + "account/login", cred).pipe(
            tap(user => {
                this.setCurrentUser(user);
            })
        )
    }

    setCurrentUser(user : User){
        localStorage.setItem("user", JSON.stringify(user));
        this.currentUser.set(user);
        this.likeService.getLikeIds();
    }

    logout(){
        this.currentUser.set(null);
        localStorage.removeItem("user");
        this.likeService.clearLikeIds();
        localStorage.removeItem("filters");
    }
}

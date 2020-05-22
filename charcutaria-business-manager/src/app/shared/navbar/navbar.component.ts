import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth/auth.service';
import { UserService } from '../services/user/user.service';
import { ParentModule } from '../models/parentModule';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {

  public companyName: string;
  public modules: ParentModule[] = [];
  constructor(private userService: UserService,
    private authService: AuthService) { }

  ngOnInit(): void {
    this.companyName = this.authService.userData.companyName;
    this.userService.GetUserModules(this.authService.userData.userId).subscribe(m => {
      this.modules = m;
    });
  }

}

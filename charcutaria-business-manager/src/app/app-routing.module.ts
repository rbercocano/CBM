import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AdminComponent } from './layout/admin/admin.component';
import { AuthComponent } from './layout/auth/auth.component';
import { CustomerComponent } from './layout/customer/customer.component';
import { AuthGuard } from './shared/guards/AuthGuard';
import { ChildAuthGuard } from './shared/guards/ChildAuthGuard';



const routes: Routes = [{
  canActivate: [AuthGuard],
  path: '',
  component: CustomerComponent,
  canActivateChild: [ChildAuthGuard],
  children: [
    {
      path: '',
      redirectTo: 'dashboard',
      pathMatch: 'full'
    },
    {
      path: 'dashboard',
      loadChildren: () => import('./pages/customer/dashboard/dashboard.module').then(m => m.DashboardModule)
    },
    {
      path: 'user',
      loadChildren: () => import('./pages/customer/user/user.module').then(m => m.UserModule)
    },
    {
      path: 'product',
      loadChildren: () => import('./pages/customer/product/product.module').then(m => m.ProductModule)
    },
    {
      path: 'customer',
      loadChildren: () => import('./pages/customer/customer/customer.module').then(m => m.CustomerModule)
    },
    {
      path: 'order',
      loadChildren: () => import('./pages/customer/order/order.module').then(m => m.OrderModule)
    },
    {
      path: 'raw-material',
      loadChildren: () => import('./pages/customer/raw-material/raw-material.module').then(m => m.RawMaterialModule)
    },
    {
      path: 'report',
      loadChildren: () => import('./pages/customer/report/report.module').then(m => m.ReportModule)
    }]
},
{
  canActivate: [AuthGuard],
  path: 'system',
  component: AdminComponent,
  children: [
    {
      path: 'modules',
      loadChildren: () => import('./pages/customer/dashboard/dashboard.module').then(m => m.DashboardModule)
    }]
},
{
  path: 'auth',
  component: AuthComponent,
  children: [
    {
      path: '',
      loadChildren: () => import('./pages/auth/auth.module').then(m => m.AuthModule)
    }]
}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

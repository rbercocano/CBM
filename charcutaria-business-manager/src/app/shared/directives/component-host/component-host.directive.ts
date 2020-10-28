import { AfterViewInit, Compiler, ComponentFactoryResolver, Directive, Injector, Input, Type, ViewContainerRef } from '@angular/core';
import { SharedModule } from '../../shared.module';

@Directive({
  selector: '[componentHost]'
})
export class ComponentHostDirective implements AfterViewInit {
  @Input("componentName") componentName: string;
  @Input("data") data: [{ key, value }];
  constructor(public templateViewContainerRef: ViewContainerRef,
    private componentFactoryResolver: ComponentFactoryResolver,
    private compiler: Compiler) { }
  ngAfterViewInit(): void {
    this.loadComponents();
  }
  loadComponents() {
    let cName = 'total-orders';
    import('src/app/shared/shared.module').then(
      (module) => {
        var moduleFactory = this.compiler.compileModuleAndAllComponentsSync(SharedModule);
        var cFactory = moduleFactory.componentFactories.filter(c => c.selector == this.componentName)[0];
        const component = this.componentFactoryResolver.resolveComponentFactory(cFactory.componentType);
        const componentRef = this.templateViewContainerRef.createComponent(component);
        this.templateViewContainerRef.createComponent
        // this.data.forEach((v, i) => {
        //   componentRef.instance[v.key] = v.value;
        // })
      }
    );
  }
}
